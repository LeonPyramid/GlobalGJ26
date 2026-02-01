using System;
using System.Collections;
using System.Collections.Generic;
using Audio;
using DG.Tweening;
using Script.UI.BinUI;
using UnityEngine;
using Random = UnityEngine.Random;

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

    [SerializeField] private List<Sprite> binFaceEmpty;
    [SerializeField] private List<Sprite> binFaceFull;


    private Player ?player;

    [SerializeField] private BinUI binUI;
    // Use Play fill to play fill, Stop fill to stop it, values can be changed in Bin prefab, deactivate Canvas if you don't like the system

    [Header("Shake vars")]

    [SerializeField] private float shakeTime;
    [SerializeField] int vibrato;
    [SerializeField] int force;
    [SerializeField] int randomness;



    public static Action OnKeyPickedUp;
    private void Awake()
    {
        //TODO Rajouter Random sur sprite poubelle
        timeManager = TimeManager.Instance;
        //childSprite = GetComponentInChildren<SpriteRenderer>();
        childSprite.enabled = false;
        player = null;
    }



    public override void SetBlocked()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = binFaceFull[UnityEngine.Random.Range(0, binFaceFull.Count)];
    }

    public override void SetUnblocked()
    {
            gameObject.GetComponent<SpriteRenderer>().sprite = binFaceEmpty[UnityEngine.Random.Range(0, binFaceFull.Count)];
            childSprite.sprite = gameObject.GetComponent<SpriteRenderer>().sprite;


    }

    override public void ActionEffect(Collider2D playerCollider)
    {
        
        if (!isUsable) return;
        BinShake();
        GameObject playerGo = playerCollider.gameObject.GetComponent<PlayerGrasp>().player.gameObject;
        playerGo.transform.position = transform.position;
        if (player == null)
            player = playerGo.GetComponent<Player>(); 
        player.SetHidden();
        binUI.PlayFill();
        player.OnClick += LeaveBin;
        isPlayerInside = true;
        timeManager.SetNewTimeSpeed(TimeManager.NewTimeType.Moving);
        StartCoroutine(ProcessEjectCoolDown(player));
        if (hasKey)
        {
            OnKeyPickedUp.Invoke();
            hasKey = false;
        }
    }

    IEnumerator ProcessUseCoolDown()
    {
        PlayerInteraction pi = GetComponentInParent<PlayerInteraction>();
        pi.status = PlayerInteraction.Status.Blocked;
        isUsable = false;
        Debug.Log("Je peux plus");
        yield return new WaitForSecondsRealtime(useCoolDown);
        Debug.Log("Je use");
        isUsable = true;
        pi.status = PlayerInteraction.Status.Far;
    }

    void LeaveBin(){
        StopAllCoroutines();
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
        player.OnClick -= LeaveBin;
        BinShake();
        Debug.Log("Pop!");
        EjectPlayer(player);
        binUI.StopFill();
        isPlayerInside = false;
        StartCoroutine(ProcessUseCoolDown());
    }

    public override void TimerEffect(Collider2D ?playerCollider)
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

    void BinShake(){
        AudioController.Instance.PlayAudio(Audio.AudioType.SFX_Bin, false, 0f, Random.Range(.8f,1f));
        transform.DOShakeRotation(shakeTime,new Vector3(0,0,force),vibrato,randomness).SetUpdate(true);
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
