using UnityEngine;

public class Cop : MonoBehaviour
{
    private PolygonCollider2D visionCone;
    [SerializeField] private float rotatingSpeed = 0.65f;

    void Awake()
    {
        visionCone = this.GetComponent<PolygonCollider2D>();
    }

    void FixedUpdate()
    {
        transform.Rotate(0, 0, rotatingSpeed, Space.Self);
    }

    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("In range!");
        Status status = 
    }*/


}
