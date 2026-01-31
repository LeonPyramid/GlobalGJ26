using DG.Tweening;
using Unity.VisualScripting;
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
    private Tween lastMove;
    /// <summary>
    /// The player's collider, needed to get its dimension
    /// </summary>
    private CircleCollider2D cCollider;
    private float radius;

    /// <summary>
    ///  The player's current status
    /// </summary>
    public enum Status {Moving, Static, Hidden}

    [SerializeField] 
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
        if(mouse.leftButton.wasPressedThisFrame){
            lastMove.Kill();
            float time;
            Vector2 dest = GetDestPos(out time);
            //MoveToPos(dest,time);
            MoveDir();
        }
    }

    /// <summary>
    /// Will check when the player hits something
    /// **WARNING** is not used to check when objects are near
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("HasLanded");
        int CollLayer = collision.gameObject.layer;
        if(CollLayer==LayerMask.GetMask("Wall") || CollLayer==LayerMask.GetMask("CollidableObject")){
            lastMove.Kill();
        }
    }

    /// <summary>
    ///    Returns the max position where the player can jump, counting obstacles
    /// </summary>
    /// <returns>The new position</returns>
    private Vector2 GetDestPos(out float time){
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - (Vector2)transform.position).normalized;
    
        //Restrict raycast to collidables first
        LayerMask mask = LayerMask.GetMask("CollidableObject");
        RaycastHit2D hitPoint = Physics2D.Raycast(transform.position,direction, (dashDistance+radius),mask);
        if (hitPoint)
        {   
            Vector2 dest = hitPoint.point - (direction * cCollider.radius);
            float distance = Vector2.Distance(transform.position, dest);
            time = distance / dashSpeed;
            Debug.DrawRay(transform.position, direction * distance, Color.blue,0.5f);
            return hitPoint.point - (direction * cCollider.radius);
        }

        //Restrict to wall
        mask = LayerMask.GetMask("Wall");
        hitPoint = Physics2D.Raycast(transform.position,direction, (dashDistance+radius),mask);
        if (hitPoint)
        {
            //ExtWall.Orientation wallOrt = hitPoint.collider.gameObject.GetComponent<ExtWall>().wallOrientation;
            //TODO Terminer le calcul de la tangente si jamais on revient à DotWeen

            Vector2 dest = hitPoint.point - (direction * cCollider.radius);
            float distance = Vector2.Distance(transform.position, dest);
            time = distance / dashSpeed;
            Debug.DrawRay(transform.position, direction * distance, Color.blue,0.5f);
            return hitPoint.point - (direction * cCollider.radius);
        }
        else
        {
            Debug.DrawRay(transform.position, direction * 100, Color.yellow,0.5f);
            time = dashDistance / dashSpeed;
            return (Vector2)transform.position + (direction * dashDistance);
        }
    }

    /// <summary>
    /// Moves the object to this position
    /// </summary>
    /// <param name="pos"></param>
    private void MoveToPos(Vector2 pos,float time){
        //TODO fine tune la foction d'acceleration
        //TODO Changer le temps par une vitesse de déplacement (faire en sorte que le tps dépende de la distance)
        lastMove = transform.DOMove(pos,time).SetEase(Ease.OutQuart);
    }

    private void MoveDir()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - (Vector2)transform.position).normalized;
        GetComponent<Rigidbody2D>().AddForce(direction*dashSpeed);
    }

#region DEBUG_FUNC

[System.Diagnostics.Conditional("DEBUG")]
public void ColorStatus(){
    //TODO : change la couleur du perso en fonction de son êtat
}

#endregion
}
