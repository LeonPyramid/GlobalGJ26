using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [Header("Music")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private TextMeshProUGUI musicValue;
    private const string MusicKey = "MUSIC";
    public Action<float> OnMusicVolumeChanged;

    [Header("Sfx")]
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private TextMeshProUGUI sfxValue;
    private const string SfxKey = "SFX";
    public Action<float> OnSfxVolumeChanged;

    [Header("Sound")]
    [SerializeField] private Button soundButton;
    [SerializeField] private TextMeshProUGUI soundText;
    private const string SoundOn = "Sound: On";
    private const string SoundOff = "Sound: Off";
    private const string SoundKey = "Sound";
    private bool _currentSoundState = true;
    public Action<bool> OnSoundChanged;


    private void Start()
    {
        InitMusic();

        InitSfx();

        InitSound();

        SoundManager.Instance.Notify(this);
    }

    private void InitMusic()
    {
        musicSlider.onValueChanged.AddListener(OnMusicValueChanged);

        musicSlider.value = PlayerPrefs.GetFloat(SoundManager.MusicKey, 1f);

        musicValue.text = ((int)(musicSlider.value * 100)).ToString();
    }

    private void InitSfx()
    {
        sfxSlider.onValueChanged.AddListener(OnSfxValueChanged);

        sfxSlider.value = PlayerPrefs.GetFloat(SoundManager.SfxKey, 1f);

        sfxValue.text = ((int)(sfxSlider.value * 100)).ToString();
    }

    private void InitSound()
    {
        soundButton.onClick.AddListener(SoundButtonClicked);

        _currentSoundState = PlayerPrefs.GetInt(SoundManager.SoundKey, 1) != 0;

        if(_currentSoundState)
        {
            soundText.text = SoundOn;
        } 
        else
        {
            soundText.text  = SoundOff;
        }
    }

    private void SoundButtonClicked()
    {
        if(_currentSoundState)
        {
            _currentSoundState = false;

            soundText.text = SoundOff;
        } 
        else
        {
            _currentSoundState = true;

            soundText.text = SoundOn;
        }

        PlayerPrefs.SetInt(SoundKey, _currentSoundState? 1 : 0);

        OnSoundChanged?.Invoke(_currentSoundState);
    }

    private void OnMusicValueChanged(float value)
    {
        musicValue.text = ((int)(value * 100)).ToString();

        PlayerPrefs.SetFloat(MusicKey, value);

        OnMusicVolumeChanged?.Invoke(value);
    }

    private void OnSfxValueChanged(float value)
    {
        sfxValue.text = ((int)(value * 100)).ToString();

        PlayerPrefs.SetFloat(SfxKey, value);

        OnSfxVolumeChanged?.Invoke(value);
    }
}