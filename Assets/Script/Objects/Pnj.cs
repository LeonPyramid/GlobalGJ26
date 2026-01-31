using UnityEngine;

public class Pnj : PlayerInteraction.PlayerAction
{
   private TimeManager timeManager;

    private void Start()
    {
        timeManager = TimeManager.Instance;
    }

    override public void ActionEffect(Collider2D playerCollider){
    }
    public override void TimerEffect(Collider2D playerCollider)
    {
        Vector2 playerDir = playerCollider.gameObject.GetComponent<PlayerGrasp>().player.direction;
        Vector2 playerToObjDir = (transform.position - playerCollider.gameObject.transform.position).normalized;
        float quality = playerDir.x  * playerToObjDir.x +  playerDir.y  * playerToObjDir.y ;
        timeManager.AddQualityQTE(quality);
        timeManager.SetNewTimeSpeed(TimeManager.NewTimeType.QTE);
    }

    public override void TimerRevert()
    {
        //
    }
}
