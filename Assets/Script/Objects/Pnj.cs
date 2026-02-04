using Unity.VisualScripting;
using UnityEngine;

public class Pnj : MonoBehaviour,  IInteractable
{
   private TimeManager timeManager;
    [SerializeField] private GameObject richPart;

    private bool hasMoney;
    //public bool HasMoney => {set };
    public bool CanInteract => hasMoney;

    private void Start()
    {
        timeManager = TimeManager.Instance;
    }

    #nullable enable
    public void OnPlayerEnter(Collider2D playerCollider)
    {
        if (!CanInteract) return;

        GameManager.Instance.ChangeGameState(GameState.Qte);

        // Calcul de la qualité du vol (Direction)
        var playerGrasp = playerCollider.GetComponent<PlayerGrasp>();
        if (playerGrasp != null)
        {
            Vector2 playerDir = (Vector2)playerGrasp.player.direction;
            Vector2 playerToObjDir = (transform.position - playerCollider.transform.position).normalized;
            float quality = Vector2.Dot(playerDir, playerToObjDir);

            TimeManager.Instance.AddQualityQTE(quality);
        }

        QteBehaviour.Instance.OnDone += PostQTE;
        TimeManager.Instance.SetNewTimeSpeed(TimeManager.NewTimeType.QTE);
    }

    public void OnPlayerExit()
    {

    }
#nullable disable

    public void PostQTE(int score)
    {
        if (score > 0)
        {
            SetAvailability(false);
        }
        QteBehaviour.Instance.OnDone -= PostQTE;
    }

    public void SetAvailability(bool isAvailable)
    {
        hasMoney = isAvailable;
        richPart.SetActive(isAvailable);
    }

    public void ExecuteAction()
    {
        throw new System.NotImplementedException();
    }

    public void OnPlayerEnter(Player player)
    {
        throw new System.NotImplementedException();
    }
}
