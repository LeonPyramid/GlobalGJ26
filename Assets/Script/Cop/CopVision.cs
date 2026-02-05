using Audio;
using System;
using System.Linq;
using UnityEngine;

public class CopVision : MonoBehaviour
{
    private PolygonCollider2D visionCone;
    [SerializeField] private float rotatingSpeed = 0.65f;
    [SerializeField] private LayerMask mask;
 
    public enum Status { Idle, Chasing };

    [SerializeField] public Status status;
    private Transform target;
    private Cop cop;


    void Awake()
    {
        visionCone = this.GetComponent<PolygonCollider2D>();
        cop = GetComponentInParent<Cop>();
    }

    void FixedUpdate()
    {
        if (cop.IsStatic())
            transform.Rotate(0, 0, rotatingSpeed * Time.deltaTime, Space.Self);

        else if(target !=  null)
        {
            Vector2 direction = (cop.GetTarget().position - transform.position).normalized;
            transform.rotation = Quaternion.FromToRotation(new Vector2(0, 1), direction);
        }
        else if(status == Status.Idle) 
        {
            target = cop.GetTarget();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Is in vision");
        int CollLayer = collision.gameObject.layer;
        if(CollLayer==LayerMask.NameToLayer("PlayerBody")){
            Player player = collision.gameObject.GetComponent<Player>();
            if(player.status != Player.Status.Hidden){
                Vector2 direction = ((Vector2)player.transform.position - (Vector2)transform.position).normalized;
                RaycastHit2D hit = Physics2D.Raycast( (Vector2)this.transform.position,direction,Mathf.Infinity, mask);
                Debug.Log(hit);
                if(hit.collider.gameObject.layer == LayerMask.NameToLayer("PlayerBody") && status != Status.Chasing){
                    target = player.transform;
                    status = Status.Chasing;
                    cop.BeginChase(target.transform);
                    AudioController.Instance.PlayAudio(Audio.AudioType.SFX_CopWhistle);
                }
            }
        }
    }


}
