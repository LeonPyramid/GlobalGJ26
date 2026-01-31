using System;
using Audio;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [Header("Music")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private TextMeshProUGUI musicValue;
    public Action<float> OnMusicVolumeChanged;

    [Header("Sfx")]
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private TextMeshProUGUI sfxValue;
    public Action<float> OnSfxVolumeChanged;

    [Header("Sound")]
    [SerializeField] private Button soundButton;
    [SerializeField] private TextMeshProUGUI soundText;
    private const string SoundOn = "Sound: On";
    private const string SoundOff = "Sound: Off";
    private bool _mute;
    public Action<bool> OnSoundChanged;


    private void Start()
    {
        InitMusic();

        InitSfx();

        InitSound();
    }

    private void InitMusic()
    {
        musicSlider.onValueChanged.AddListener(OnMusicValueChanged);

        musicSlider.value = PlayerPrefs.GetFloat(nameof(AudioBusType.Music));

        musicValue.text = ((int)(musicSlider.value * 100)).ToString();
    }

    private void InitSfx()
    {
        sfxSlider.onValueChanged.AddListener(OnSfxValueChanged);

        sfxSlider.value = PlayerPrefs.GetFloat(nameof(AudioBusType.SFX));

        sfxValue.text = ((int)(sfxSlider.value * 100)).ToString();
    }

    private void InitSound()
    {
        soundButton.onClick.AddListener(SoundButtonClicked);

        _mute = PlayerPrefs.GetInt(AudioController.MuteKey, 1) == 1;

        if(_mute)
        {
            soundText.text = SoundOff;
        } 
        else
        {
            soundText.text  = SoundOn;
        }
    }

    private void SoundButtonClicked()
    {
        if(_mute)
        {
            _mute = false;

            soundText.text = SoundOn;
        } 
        else
        {
            _mute = true;

            soundText.text = SoundOff;
        }

        AudioController.Instance?.ChangeAudioState(_mute);
    }

    private void OnMusicValueChanged(float value)
    {
        musicValue.text = ((int)(value * 100)).ToString();

        AudioController.Instance?.SetBusVolume(AudioBusType.Music, value);
    }

    private void OnSfxValueChanged(float value)
    {
        sfxValue.text = ((int)(value * 100)).ToString();

        AudioController.Instance?.SetBusVolume(AudioBusType.SFX, value);
    }
}