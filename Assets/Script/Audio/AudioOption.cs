using Audio;
using UnityEngine;
using UnityEngine.Serialization;
using Slider = UnityEngine.UI.Slider;

namespace UnityCore.Audio
{
    public class AudioOption : MonoBehaviour
    {
        [SerializeField] Slider music;
        [SerializeField] Slider SFX;

        // Start is called before the first frame update
        void Awake()
        {
            music.value = PlayerPrefs.GetFloat(nameof(AudioBusType.Music));
            SFX.value = PlayerPrefs.GetFloat(nameof(AudioBusType.SFX));
        }
        
        void OnEnable()
        {
            music.onValueChanged.AddListener(delegate { UpdateMusic(); });
            SFX.onValueChanged.AddListener(delegate { UpdateSfx(); });
            UpdateMusic();
            UpdateSfx();
        }
        
        void OnDisable()
        {
            music.onValueChanged.RemoveAllListeners();
            SFX.onValueChanged.RemoveAllListeners();
        }

        void Update()
        {
            // UpdateMusic();
            // UpdateSfx();
        }


        public void UpdateMusic()
        {
            PlayerPrefs.SetFloat(nameof(AudioBusType.Music), music.value);
            AudioController.Instance?.SetBusVolume(AudioBusType.Music, music.value);
        }

        public void UpdateSfx()
        {
            PlayerPrefs.SetFloat(nameof(AudioBusType.SFX), SFX.value);
            AudioController.Instance?.SetBusVolume(AudioBusType.SFX, SFX.value);
            
            Debug.Log("SFX registered volume: " + PlayerPrefs.GetFloat("SFX"));
        }
    }
}