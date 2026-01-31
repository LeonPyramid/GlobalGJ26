using UnityEngine;
using Utils.Singleton;

public class SoundManager : Singleton<SoundManager>
{
    [Header("Music")]
    [SerializeField] private AudioSource musicAudioSource;
    public const string MusicKey = "MUSIC";

    [Header("Sfx")]
    [SerializeField] private AudioSource sfxAudioSource;
    [SerializeField] private AudioClip click;
    public const string SfxKey = "SFX";

    public const string SoundKey = "Sound";


    private void Start()
    {
        InitMusic();

        InitSfx();

        InitSoundState();
    }

    private void InitMusic()
    {
        musicAudioSource.volume = PlayerPrefs.GetFloat(MusicKey, 1f);
    }

    private void InitSfx()
    {
        musicAudioSource.volume = PlayerPrefs.GetFloat(SfxKey, 1f);
    }

    private void InitSoundState()
    {
        var currentSoundState = PlayerPrefs.GetInt(SoundKey, 1) != 0;

        musicAudioSource.enabled = currentSoundState;

        sfxAudioSource.enabled = currentSoundState;
    }

    public void Notify(SettingsMenu settingsMenu)
    {
        settingsMenu.OnMusicVolumeChanged += OnMusicVolumeChanged;

        settingsMenu.OnSfxVolumeChanged += OnSfxVolumeChanged;

        settingsMenu.OnSoundChanged += OnSoundStateChanged;
    }

    private void OnMusicVolumeChanged(float value)
    {
        musicAudioSource.volume = value;
    }

    private void OnSfxVolumeChanged(float value)
    {
        sfxAudioSource.volume = value;
    }

    private void OnSoundStateChanged(bool value)
    {
        musicAudioSource.enabled = value;

        sfxAudioSource.enabled = value;
    }

    #region music

    // put music code

    #endregion

    #region sfx

    public void PlayClick()
    {
        if(!sfxAudioSource.enabled) return;

        sfxAudioSource.PlayOneShot(click);
    }

    #endregion
}