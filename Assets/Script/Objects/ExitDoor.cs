using System;
using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    public static Action<bool> OnPlayerExit;

    bool isOpen = false;

    void OnEnable()
    {
        Bin.OnKeyPickedUp += UnlockDoor;
    }

    private void OnDisable()
    {
        Bin.OnKeyPickedUp -= UnlockDoor;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isOpen)
            return;
        int CollLayer = collision.gameObject.layer;
        if (CollLayer == LayerMask.NameToLayer("PlayerBody"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (player.status != Player.Status.Hidden)
            {
                OnPlayerExit.Invoke(true);
            }
        }
    }

    void UnlockDoor()
    {
        isOpen = true;
        //TODO add FX
    }
}
