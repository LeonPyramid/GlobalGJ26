using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bin : PlayerInteraction.PlayerAction
{
    private TimeManager timeManager;
    bool hasKey = false;
    [SerializeField] float useCoolDown = 10;
    [SerializeField] float ejectCoolDown = 3;
    bool isUsable = true;
    bool isPlayerInside;

    public static Action OnKeyPickedUp;
    private void Awake()
    {
        timeManager = TimeManager.Instance;
    }

    override public void ActionEffect(Collider2D playerCollider)
    {
        if (!isUsable) return;
        GameObject playerGo = playerCollider.gameObject.GetComponent<PlayerGrasp>().player.gameObject;
        playerGo.transform.position = transform.position;
        Player player = playerGo.GetComponent<Player>();
        player.SetHidden();
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
        yield return new WaitForSeconds(useCoolDown);
        isUsable = true;
    }

    IEnumerator ProcessEjectCoolDown(Player player)
    {
        yield return new WaitForSeconds(ejectCoolDown);
        if (isPlayerInside)
        {
            EjectPlayer(player);
            isPlayerInside = false;
        }
    }
    public override void TimerEffect(Collider2D playerCollider)
    {
        if (!isUsable) return;
        timeManager.SetNewTimeSpeed(TimeManager.NewTimeType.Bin);
    }

    public override void TimerRevert()
    {
        timeManager.PopTypeSpeed(TimeManager.NewTimeType.Bin);
        timeManager.PopTypeSpeed();
        StartCoroutine(ProcessUseCoolDown());
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
