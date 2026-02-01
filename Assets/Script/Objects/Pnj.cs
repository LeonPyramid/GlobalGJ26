using Unity.VisualScripting;
using UnityEngine;

public class Pnj : PlayerInteraction.PlayerAction
{
   private TimeManager timeManager;
    [SerializeField] private GameObject richPart;

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
        QteBehaviour.Instance.OnDone += PostQTE;
        timeManager.AddQualityQTE(quality);
        timeManager.SetNewTimeSpeed(TimeManager.NewTimeType.QTE);
    }
    #nullable disable
    public override void SetBlocked()
    {
        richPart.SetActive(false);
    }

        public override void SetUnblocked()
    {
        richPart.SetActive(true);

    }

    public override void TimerRevert()
    {
        //
    }

    public void PostQTE(int score){
        if (score > 0){
            GetComponentInParent<PlayerInteraction>().SetBLocked();
        }
        QteBehaviour.Instance.OnDone -= PostQTE;
    }
}
