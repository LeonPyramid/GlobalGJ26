using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    /// <summary>
    /// The max distance the player can dash
    /// </summary>
    [SerializeField] private float dashDistance;
    /// <summary>
    /// The time of the dash
    /// </summary>
    [SerializeField] private float dashSpeed;
    /// <summary>
    /// The player's collider, needed to get its dimension
    /// </summary>
    private CircleCollider2D cCollider;
    private float radius;
    Camera m_Camera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        m_Camera = Camera.main;
        cCollider = this.GetComponent<CircleCollider2D>();
        radius = cCollider.radius;

    }

    // Update is called once per frame
    void Update()
    {
            Mouse mouse = Mouse.current;
            if(mouse.leftButton.wasPressedThisFrame)
                MoveToPos(GetDestPos());
    }

    /// <summary>
    ///    Returns the max position where the player can jump, counting obstacles
    /// </summary>
    /// <returns>The new position</returns>
    private Vector2 GetDestPos(){
        //TODO rajouter masque de layers pour diff objets
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - (Vector2)transform.position).normalized;

        //Restrict raycast to walls
        LayerMask mask = LayerMask.GetMask("Wall");
        RaycastHit2D hitPoint = Physics2D.Raycast(transform.position,direction, (dashDistance+radius),mask);

        Debug.Log(hitPoint);
        if (hitPoint)
        {   
            Vector2 dest = hitPoint.point - (direction * cCollider.radius);
            Debug.DrawRay(transform.position, direction * (Vector2.Distance(transform.position,dest)), Color.blue,0.5f);
            return hitPoint.point - (direction * cCollider.radius);
        }
        else
        {
            Debug.DrawRay(transform.position, direction * 100, Color.yellow,0.5f);
            return (Vector2)transform.position + (direction * dashDistance);
        }
    }

    /// <summary>
    /// Moves the object to this position
    /// </summary>
    /// <param name="pos"></param>
    private void MoveToPos(Vector2 pos){
        //TODO fine tune la foction d'acceleration
        transform.DOMove(pos,dashSpeed).SetEase(Ease.OutSine);
    }


#region DEBUG_FUNC

[System.Diagnostics.Conditional("DEBUG")]
public void ColorStatus(){
    //TODO : change la couleur du perso en fonction de son êtat
}

#endregion
}
