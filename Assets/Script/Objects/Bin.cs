using UnityEngine;

public class Bin : PlayerInteraction.PlayerAction
{
    override public void ActionEffect(Collider2D playerCollider){
        GameObject playerGo = playerCollider.gameObject.GetComponent<PlayerGrasp>().player.gameObject;
        playerGo.transform.position = transform.position;
        playerGo.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        playerGo.GetComponent<Rigidbody2D>().angularVelocity = 0;
    }
}
