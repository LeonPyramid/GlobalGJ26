using UnityEngine;

public class Bin : PlayerInteraction.PlayerAction
{
    [SerializeField] private SpriteRenderer childSprite;
    private TimeManager timeManager;

    private void Awake()
    {
        //TODO Rajouter Random sur sprite poubelle
        timeManager = TimeManager.Instance;
        //childSprite = GetComponentInChildren<SpriteRenderer>();
        childSprite.sprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        childSprite.enabled = false;
    }

    override public void ActionEffect(Collider2D playerCollider){
        GameObject playerGo = playerCollider.gameObject.GetComponent<PlayerGrasp>().player.gameObject;
        playerGo.transform.position = transform.position;
        playerGo.GetComponent<Player>().SetStatic();
    }
    public override void TimerEffect(Collider2D playerCollider)
    {
        childSprite.enabled = true;
        timeManager.SetNewTimeSpeed(TimeManager.NewTimeType.Bin);
    }

    public override void TimerRevert()
    {
        childSprite.enabled = false;
        timeManager.PopTypeSpeed(TimeManager.NewTimeType.Bin);
    }
}
