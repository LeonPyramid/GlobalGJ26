using UnityEngine;

public class Bin : PlayerInteraction.PlayerAction
{
    private TimeManager timeManager;

    private void Awake()
    {
        timeManager = TimeManager.Instance;
    }

    override public void ActionEffect(Collider2D playerCollider){
        GameObject playerGo = playerCollider.gameObject.GetComponent<PlayerGrasp>().player.gameObject;
        playerGo.transform.position = transform.position;
        playerGo.GetComponent<Player>().SetStatic();
    }
    public override void TimerEffect(Collider2D playerCollider)
    {
        timeManager.SetNewTimeSpeed(TimeManager.NewTimeType.Bin);
    }

    public override void TimerRevert()
    {
        timeManager.PopTypeSpeed(TimeManager.NewTimeType.Bin);
    }
}
