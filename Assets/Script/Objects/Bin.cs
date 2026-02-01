using System;
using System.Collections;
using System.Collections.Generic;
using Script.UI.BinUI;
using UnityEngine;

public class Bin : PlayerInteraction.PlayerAction
{
    [SerializeField] private SpriteRenderer childSprite;
    private TimeManager timeManager;
    bool hasKey = false;
    [SerializeField] public float useCoolDown 
    {
        get; private set;
    } = 10;
    [SerializeField]
    public float ejectCoolDown
    {
        get; private set;
    } = 3;
    bool isUsable = true;
    bool isPlayerInside;

    private Player ?player;


    [SerializeField] private BinUI binUI;
    // Use Play fill to play fill, Stop fill to stop it, values can be changed in Bin prefab, deactivate Canvas if you don't like the system

    public static Action OnKeyPickedUp;
    private void Awake()
    {
        //TODO Rajouter Random sur sprite poubelle
        timeManager = TimeManager.Instance;
        //childSprite = GetComponentInChildren<SpriteRenderer>();
        childSprite.sprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        childSprite.enabled = false;
        player = null;
    }

    override public void ActionEffect(Collider2D playerCollider)
    {
        
        if (!isUsable) return;
        GameObject playerGo = playerCollider.gameObject.GetComponent<PlayerGrasp>().player.gameObject;
        playerGo.transform.position = transform.position;
        if (player == null)
            player = playerGo.GetComponent<Player>(); 
        player.SetHidden();
        binUI.PlayFill();
        player.OnClick += LeaveBin;
        isPlayerInside = true;
        StartCoroutine(ProcessEjectCoolDown(player));
        if (hasKey)
        {
            OnKeyPickedUp.Invoke();
            hasKey = false;
        }
    }

    IEnumerator ProcessUseCoolDown()
    {
        isUsable = false;
        Debug.Log("Je peux plus");
        yield return new WaitForSecondsRealtime(useCoolDown);
        Debug.Log("Je use");
        isUsable = true;
    }

    void LeaveBin(){
        StopAllCoroutines();
        player.OnClick -= LeaveBin;
        Debug.Log("Je quitte");
        ProcessEjectInstant(player);
    }


    IEnumerator ProcessEjectCoolDown(Player player)
    {
        yield return new WaitForSecondsRealtime(ejectCoolDown);
        if (isPlayerInside)
        {
            ProcessEjectInstant(player);
        }
    }

    void ProcessEjectInstant(Player player){
            Debug.Log("Pop!");
            EjectPlayer(player);
            binUI.StopFill();
            isPlayerInside = false;
            StartCoroutine(ProcessUseCoolDown());
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
        //StartCoroutine(ProcessUseCoolDown());
    }

    public void SetHasKey()
    {
        hasKey = true;
    }
   
    private void EjectPlayer(Player player)
    {
        player.SetStatic();
    }
}
