using Unity.VisualScripting;
using UnityEngine;

public class CopKnock : InteractationBehaviour
{
   private TimeManager timeManager;
    //[SerializeField] private GameObject richPart;

    private void Start()
    {
        timeManager = TimeManager.Instance;
    }

    #nullable enable
    //TODO implémenter l'interface IInsteractable
    public void TimerEffect(Collider2D ?playerCollider)
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

    public void PostQTE(int score){
        if (score > 0){
            //GetComponentInParent<PlayerInteraction>().SetBLocked();
        }
        QteBehaviour.Instance.OnDone -= PostQTE;
    }
}
