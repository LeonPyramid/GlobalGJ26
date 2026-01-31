using UnityEngine;
using UnityEngine.UI;

public class ButtonSoundBehaviour : MonoBehaviour
{
    [SerializeField] private Button button;

    private void Start()
    {
        button.onClick.AddListener(PlaySound);
    }

    private void PlaySound()
    {
        SoundManager.Instance.PlayClick();
    }
}