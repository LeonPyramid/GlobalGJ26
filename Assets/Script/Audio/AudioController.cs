using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using Utils.Singleton;


namespace Audio
{
    public enum AudioBusType
    {
        Music,
        SFX
    }

    public enum AudioType
    {
        None,
        Music_Menu,
        Music_Gameplay,
        SFX_ButtonPressed,
        SFX_ButtonHover,
        SFX_GameOver,
        SFX_PlaceTile,
        ST_Main,
        SFX_TileScoring,
        SFX_AddScore,
        SFX_ScoreCounter,
        SFX_NewBestScore
    }

    public class AudioController : Singleton<AudioController>
    {
        public bool debug;

        [Header("Audio Configuration")] public AudioTrack[] tracks;
        public List<AudioBus> audioBuses = new();

        private Hashtable m_AudioTable;
        private Hashtable m_JobTable;
        private Dictionary<AudioBusType, AudioBus> busDict = new();

        public static Action OnConfigure;

        [System.Serializable]
        public class AudioObject
        {
            public AudioType type;
            public AudioClip clip;
        }

        [System.Serializable]
        public class AudioTrack
        {
            public AudioBusType busType;
            public AudioSource source;
            public AudioObject[] audio;
            public bool paused = false;
            public float volume = 1f;
            public float pitch = 1f;
        }

        [System.Serializable]
        public class AudioBus
        {
            public AudioBusType busType;
            public List<AudioSource> sources = new();
            public float volume = 1f;

            public void SetVolume(float newVolume)
            {
                volume = newVolume;
                foreach (var src in sources)
                {
                    if (src != null)
                        src.volume = volume;
                }

                PlayerPrefs.SetFloat(busType.ToString(), volume);
            }

            public void LoadVolume()
            {
                volume = PlayerPrefs.GetFloat(busType.ToString(), 1f);
                SetVolume(volume);
            }
        }

        private class AudioJob
        {
            public AudioAction action;
            public AudioType type;
            public bool fade;
            public float delay;
            public float pitch;

            public AudioJob(AudioAction _action, AudioType _type, bool _fade, float _delay, float _pitch)
            {
                action = _action;
                type = _type;
                fade = _fade;
                delay = _delay;
                pitch = _pitch;
            }
        }

        private enum AudioAction
        {
            START,
            STOP,
            RESTART,
            PAUSE
        }

        #region Unity Lifecycle

        protected override void Awake()
        {
            base.Awake();
            Configure();
        }

        private void OnDisable()
        {
            if(Instance != null)
                Dispose();
        }

        #endregion

        #region Initialization

        void Configure()
        {
            // Initialisation du volume
            m_AudioTable = new Hashtable();
            m_JobTable = new Hashtable();
            
            foreach (var bus in audioBuses)
            {
                busDict[bus.busType] = bus;
            }
            
            foreach (var track in tracks)
            {
                if (busDict.TryGetValue(track.busType, out var bus))
                {
                    bus.sources.Add(track.source);
                }

                foreach (var obj in track.audio)
                {
                    if (m_AudioTable.ContainsKey(obj.type))
                    {
                        Debug.LogWarning($"Audio type [{obj.type}] already registered.");
                    }
                    else
                    {
                        m_AudioTable.Add(obj.type, track);
                    }
                }
            }
            
            foreach (var bus in audioBuses)
            {
                bus.LoadVolume();
            }

            OnConfigure?.Invoke();
        }

        void Dispose()
        {
            foreach (DictionaryEntry entry in m_JobTable)
            {
                StopCoroutine((IEnumerator)entry.Value);
            }

            m_JobTable.Clear();
        }

        #endregion

        #region Audio Playback

        IEnumerator RunAudioJob(AudioJob job)
        {
            yield return new WaitForSecondsRealtime(job.delay);

            if (!m_AudioTable.ContainsKey(job.type)) yield break;

            AudioTrack track = (AudioTrack)m_AudioTable[job.type];
            track.source.clip = GetClipFromTrack(job.type, track);

            Debug.Log($"[RunAudioJob] Starting action: {job.action} for {job.type}");

            switch (job.action)
            {
                case AudioAction.START:
                    track.paused = false;
                    track.source.pitch = job.pitch;
                    track.source.Play();
                    break;

                case AudioAction.STOP:
                    if (!job.fade)
                        track.source.Stop();
                    break;

                case AudioAction.RESTART:
                    if (track.paused)
                    {
                        track.source.UnPause();
                        track.paused = false;
                    }
                    else
                    {
                        track.source.Stop();
                        track.source.Play();
                    }
                    break;

                case AudioAction.PAUSE:
                    if (!job.fade)
                    {
                        track.source.Pause();
                        track.paused = true;
                    }
                    break;
            }

            if (job.fade)
            {
                yield return Fade(track, job.action);
            }
            

            m_JobTable.Remove(job.type);
        }

        IEnumerator Fade(AudioTrack track, AudioAction action)
        {
            float initial = (action == AudioAction.START || action == AudioAction.RESTART) ? 0f : track.volume;
            float target = (action == AudioAction.START || action == AudioAction.RESTART) ? track.volume : 0f;
            float duration = 1f; // Durée du fade
            float timer = 0f;

            Debug.Log($"[Fade] Fading from {initial} to {target} for {track.source.clip.name}");
            
            if (initial == target)
            {
                Debug.Log("[Fade] No fade needed, volume already at target.");
                yield break;
            }

            // Fade In or Out
            while (timer < duration)
            {
                track.source.volume = Mathf.Lerp(initial, target, timer / duration);
                timer += Time.deltaTime;
                yield return null;
            }

            // À la fin du fade, mettre le volume au niveau cible
            track.source.volume = target;

            // Gérer l'action spécifique
            if (action == AudioAction.PAUSE)
            {
                track.source.Pause();
                track.paused = true;
            }
            else if (action == AudioAction.STOP)
            {
                track.source.Stop();
            }
            else if (action == AudioAction.START || action == AudioAction.RESTART)
            {
                // Si l'audio doit être redémarré ou joué, rétablir le volume
                track.source.volume = track.volume;
            }

            Debug.Log($"[Fade] Volume set to: {track.source.volume} after fade");
        }



        void AddJob(AudioJob job)
        {
            RemoveConflictingJobs(job.type);
            IEnumerator jobRunner = RunAudioJob(job);
            m_JobTable[job.type] = jobRunner;
            StartCoroutine(jobRunner);
        }

        void RemoveJob(AudioType type)
        {
            if (!m_JobTable.ContainsKey(type)) return;

            StopCoroutine((IEnumerator)m_JobTable[type]);
            m_JobTable.Remove(type);
        }

        void RemoveConflictingJobs(AudioType type)
        {
            if (!m_AudioTable.ContainsKey(type)) return;

            if (!(m_AudioTable[type] is AudioTrack neededTrack) || neededTrack.source == null) return;

            AudioType conflict = AudioType.None;

            foreach (DictionaryEntry entry in m_JobTable)
            {
                if (!(entry.Key is AudioType otherType)) continue;
                if (!(m_AudioTable[otherType] is AudioTrack otherTrack)) continue;
                if (otherTrack.source == null) continue;

                if (neededTrack.source == otherTrack.source)
                {
                    conflict = otherType;
                    break;
                }
            }

            if (conflict != AudioType.None)
            {
                RemoveJob(conflict);
            }
        }

        AudioClip GetClipFromTrack(AudioType type, AudioTrack track)
        {
            foreach (var obj in track.audio)
            {
                if (obj.type == type) return obj.clip;
            }

            return null;
        }

        #endregion

        #region Public API

        public void PlayAudio(AudioType type, bool fade = false, float delay = 0f, float pitch = 1f)
        {
            AddJob(new AudioJob(AudioAction.START, type, fade, delay, pitch));
            if (debug) Debug.Log($"Playing audio: {type}");
        }

        public void StopAudio(AudioType type, bool fade = false, float delay = 0f, float pitch = 1f)
        {
            AddJob(new AudioJob(AudioAction.STOP, type, fade, delay, pitch));
        }

        public void ReplayAudio(AudioType type, bool fade = false, float delay = 0f, float pitch = 1f)
        {
            AddJob(new AudioJob(AudioAction.RESTART, type, fade, delay, pitch));
        }

        public void PauseAudio(AudioType type, bool fade = false, float delay = 0f, float pitch = 1f)
        {
            AddJob(new AudioJob(AudioAction.PAUSE, type, fade, delay, pitch));
        }

        public void SetBusVolume(AudioBusType type, float volume)
        {
            if (busDict.TryGetValue(type, out var bus))
            {
                bus.SetVolume(volume);
            }
        }

        public float GetBusVolume(AudioBusType type)
        {
            return busDict.TryGetValue(type, out var bus) ? bus.volume : 1f;
        }

        #endregion
    }
}

   
