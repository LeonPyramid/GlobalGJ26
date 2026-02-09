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
                cop.Stun(1);
                Invoke(nameof(ResetCooldown), cooldownDuration);
                return;
            } 

            OnPlayerCatched?.Invoke(false);
            TimeManager.Instance.SetNewTimeSpeed(TimeManager.NewTimeType.Pause);
            AudioController.Instance.PlayAudio(Audio.AudioType.SFX_GameOver);
        }
        else 
        if(CollLayer == LayerMask.NameToLayer("PlayerRange"))
        if(GameManager.Instance.HasMaskEquiped(MaskEnum.Assassin))
            {
                 GameManager.Instance.ChangeGameState(GameState.Qte);
                Vector2 playerDir = (Vector2)(collision?.gameObject.GetComponent<PlayerGrasp>().player.direction);
                Vector2 playerToObjDir = (transform.position - collision.gameObject.transform.position).normalized;
                float quality = playerDir.x  * playerToObjDir.x +  playerDir.y  * playerToObjDir.y ;
                cooldown = true;
                Invoke(nameof(ResetCooldown), cooldownDuration);
                TimeManager.Instance.AddQualityQTE(quality);
                TimeManager.Instance.SetNewTimeSpeed(TimeManager.NewTimeType.Cop);
                QteBehaviour.OnDone += OnQteDone;   
                QteBehaviour.Instance.Show(UnityEngine.Random.Range(0,2)==0?-1f:1f, 0.25f, true);
                return;
            }
    }

    private void OnQteDone(int score)
    {
        QteBehaviour.OnDone -= OnQteDone;
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
