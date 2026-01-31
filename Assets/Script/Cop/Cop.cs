using System;
using System.Linq;
using UnityEngine;

public class Cop : MonoBehaviour
{
    private PolygonCollider2D visionCone;
    [SerializeField] private float rotatingSpeed = 0.65f;
    [SerializeField] private float chasingSpeed = 2f;
    [SerializeField] private LayerMask mask;
 
    public enum Status { Idle, Chasing };

    [SerializeField] public Status status;

    private Player target;

    void Awake()
    {
        visionCone = this.GetComponent<PolygonCollider2D>();
    }

    void FixedUpdate()
    {
        transform.Rotate(0, 0, rotatingSpeed, Space.Self);
        if(status == Status.Chasing)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, chasingSpeed);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Is in vision");
        int CollLayer = collision.gameObject.layer;
        if(CollLayer==LayerMask.NameToLayer("PlayerBody")){
            Player player = collision.gameObject.GetComponent<Player>();
            if(player.status != Player.Status.Hidden){
                //check if the player is behind a wall
                //LayerMask mask = LayerMask.GetMask("NonCollidableObject","CollidableObject", "Wall","PlayerBody");
                Vector2 direction = ((Vector2)player.transform.position - (Vector2)transform.position).normalized;
                RaycastHit2D hit = Physics2D.Raycast( (Vector2)this.transform.position,direction,Mathf.Infinity, mask);
                Debug.Log(hit);
                if(hit.collider.gameObject.layer == LayerMask.NameToLayer("PlayerBody")){
                    target = player;
                    status = Status.Chasing;}
            }
        }
    }


}
