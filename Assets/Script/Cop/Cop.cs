using UnityEngine;

public class Cop : MonoBehaviour
{
    private PolygonCollider2D visionCone;
    [SerializeField] private float rotatingSpeed = 0.65f;
    [SerializeField] private float chasingSpeed = 2f;

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
                Debug.Log("Starting chase");
                target = player;
                status = Status.Chasing;
            }
        }
    }


}
