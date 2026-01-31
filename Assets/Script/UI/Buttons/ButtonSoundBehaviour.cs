using Audio;
using UnityEngine;
using UnityEngine.UI;
using AudioType = Audio.AudioType;

public class ButtonSoundBehaviour : MonoBehaviour
{
    [SerializeField] private Button button;

    private void Start()
    {
        button.onClick.AddListener(PlaySound);
    }

    private void PlaySound()
    {
        AudioController.Instance.PlayAudio(AudioType.SFX_ButtonPressed);
    }
}