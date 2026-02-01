using Audio;
using System;
using Unity.VisualScripting;
using UnityEngine;

public class CopGrasp : MonoBehaviour
{
    public static Action<bool> OnPlayerCatched;

    float cooldownDuration = 1f;
    bool cooldown = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(cooldown) return;

        int CollLayer = collision.gameObject.layer;
        if (CollLayer == LayerMask.NameToLayer("PlayerBody"))
        {
            //Player player = collision.gameObject.GetComponent<Player>();
            // if (player.status != Player.Status.Hidden)
            // {
            if(GameManager.Instance.HasMaskEquiped(MaskEnum.Wood))
            {
                PreGameUI.Instance.ForceNewMask(MaskEnum.Basic);
                AudioController.Instance.PlayAudio(Audio.AudioType.SFX_Wood);
                cooldown = true;
                Invoke(nameof(ResetCooldown), cooldownDuration);
                return;
            }

            OnPlayerCatched?.Invoke(false);
            AudioController.Instance.PlayAudio(Audio.AudioType.SFX_GameOver);
            // }
        }
    }

    private void ResetCooldown()
    {
        cooldown = false;
    }
}
