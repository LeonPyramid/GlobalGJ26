using System;
using Unity.VisualScripting;
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
            OnPlayerExit?.Invoke(true);
        }
    }

    public void UnlockDoor()
    {
        isOpen = true;
        GetComponent<BoxCollider2D>().isTrigger = true;
        GetComponentInChildren<SpriteRenderer>().enabled = false;
        //TODO add FX
    }
}
