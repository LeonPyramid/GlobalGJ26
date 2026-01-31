using System;
using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    public static Action<bool> OnPlayerExit;
    private void OnTriggerEnter2D(Collider2D collision)
    {
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
}
