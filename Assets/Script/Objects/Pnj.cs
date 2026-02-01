using Unity.VisualScripting;
using UnityEngine;

public class Pnj : PlayerInteraction.PlayerAction
{
   private TimeManager timeManager;

    private void Start()
    {
        timeManager = TimeManager.Instance;
    }

    #nullable enable
    override public void ActionEffect(Collider2D ?playerCollider){
    }
    public override void TimerEffect(Collider2D ?playerCollider)
    {
        GameManager.Instance.ChangeGameState(GameState.Qte);
        Vector2 playerDir = (Vector2)(playerCollider?.gameObject.GetComponent<PlayerGrasp>().player.direction);
        Vector2 playerToObjDir = (transform.position - playerCollider.gameObject.transform.position).normalized;
        float quality = playerDir.x  * playerToObjDir.x +  playerDir.y  * playerToObjDir.y ;
        timeManager.AddQualityQTE(quality);
        timeManager.SetNewTimeSpeed(TimeManager.NewTimeType.QTE);
    }
    #nullable disable
    public override void SetBlocked()
    {
        throw new System.NotImplementedException();
    }

        public override void SetUnblocked()
    {
        throw new System.NotImplementedException();
    }

    public override void TimerRevert()
    {
        //
    }
}
