using Audio;
using System;
using UnityEngine;

public class CopGrasp : MonoBehaviour
{
    [SerializeField] private Cop cop;

    public static Action<bool> OnPlayerCatched;

    float cooldownDuration = 1f;
    bool cooldown = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(cooldown || GameManager.Instance.gameState == GameState.GameOver) return;

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
            else if(GameManager.Instance.HasMaskEquiped(MaskEnum.Assassin))
            {
                cooldown = true;
                Invoke(nameof(ResetCooldown), cooldownDuration);
                QteBehaviour.Instance.OnDone += OnQteDone;   
                QteBehaviour.Instance.Show(UnityEngine.Random.Range(0,2)==0?-1f:1f, 0.25f, true);
                return;
            }

            OnPlayerCatched?.Invoke(false);
            AudioController.Instance.PlayAudio(Audio.AudioType.SFX_GameOver);
        }
    }

    private void OnQteDone(int score)
    {
        if(score == 0)
        {
            OnPlayerCatched?.Invoke(false);
            AudioController.Instance.PlayAudio(Audio.AudioType.SFX_GameOver);

            return;
        }

        cooldown = true;
        cop.Stun(score);
        Invoke(nameof(ResetCooldown), cooldownDuration);
    }

    private void ResetCooldown()
    {
        cooldown = false;
    }
}
