using Audio;
using System;
using Unity.VisualScripting;
using UnityEngine;

public class CopGrasp : MonoBehaviour
{
    public static Action<bool> OnPlayerCatched;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        int CollLayer = collision.gameObject.layer;
        if (CollLayer == LayerMask.NameToLayer("PlayerBody"))
        {
            //Player player = collision.gameObject.GetComponent<Player>();
            // if (player.status != Player.Status.Hidden)
            // {
            OnPlayerCatched?.Invoke(false);
            AudioController.Instance.PlayAudio(Audio.AudioType.SFX_GameOver);
            // }
        }
    }
}
