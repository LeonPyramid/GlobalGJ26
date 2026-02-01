using DG.Tweening;
using UI.Menu;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using System;


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

    [SerializeField] private Collider2D childTrigger;

    private TimeManager timeManager;
    public Vector2 direction{
        get;private set;
    }

    public Action OnClick;

    /// <summary>
    ///  The player's current status
    /// </summary>
    public enum Status {Moving, Static, Hidden};

    [SerializeField] public Status status {
        get;set;
    } = Status.Static;
    Camera m_Camera;

    GameManager _manager;

    [SerializeField] private PlayerVisual visual;
    [SerializeField] private PlayerMaskHandler playerMaskHandler;
    [SerializeField] private float smallGraspSize;
    [SerializeField] private float bigGraspSize;
    [SerializeField] private CircleCollider2D circleCollider2D;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_Camera = Camera.main;
        cCollider = this.GetComponent<CircleCollider2D>();
        radius = cCollider.radius;
        //childTrigger = GetComponentInChildren<Collider2D>();
        timeManager = TimeManager.Instance;
        SetStatic();
        _manager = GameManager.Instance;
        playerMaskHandler.OnNewMask += OnNewMask;
    }

    private void OnNewMask(MaskEnum maskEnum)
    {
        if(maskEnum == MaskEnum.Owl)
        {
            circleCollider2D.radius = bigGraspSize;
        } 
        else
        {
            circleCollider2D.radius = smallGraspSize;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Mouse mouse = Mouse.current;
        if(mouse.leftButton.wasPressedThisFrame){
            if (MenuManager.Instance.MenuCount == 0 && GameManager.Instance.gameState == GameState.Moving){
                OnClick?.Invoke();
                if(status == Status.Moving)
                {
                    SetStatic();
                }
                //MoveToPos(dest,time);
                MoveDir();
            }
            else{
                Debug.Log("Not moving mecause a menu");
            }
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
        /*int CollLayer = collision.gameObject.layer;
        if(CollLayer==LayerMask.GetMask("Wall") || CollLayer==LayerMask.GetMask("CollidableObject")){
            lastMove.Kill();
        }*/
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        GetComponent<Rigidbody2D>().angularVelocity = 0;
        SetStatic();

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
        //lastMove = transform.DOMove(pos,time).SetEase(Ease.OutQuart);
    }

    private void MoveDir()
    {
        GameManager.Instance.AddDash();

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        direction = (mousePosition - (Vector2)transform.position).normalized;
        visual.Turn(direction.x);
        Debug.DrawRay(transform.position, direction * dashDistance, Color.yellow,0.5f);
        Debug.Log($"New Force {direction * dashSpeed} of power {(direction * dashSpeed).SqrMagnitude()}");
        GetComponent<Rigidbody2D>().linearVelocity = (direction*dashSpeed);
        SetMoving();

    }


    public void SetStatic(){
        visual.HideMode(false);
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        GetComponent<Rigidbody2D>().angularVelocity = 0;
        childTrigger.enabled = false;
        status = Status.Static;

        timeManager.SetNewTimeSpeed(TimeManager.NewTimeType.Wall);
    }

    public void SetMoving(){
        visual.HideMode(false);
        childTrigger.enabled = true;
        status = Status.Moving;

        timeManager.SetNewTimeSpeed(TimeManager.NewTimeType.Moving);

    }
    public void SetHidden(){
        visual.HideMode(true);
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        GetComponent<Rigidbody2D>().angularVelocity = 0;
        childTrigger.enabled = false;
        status = Status.Hidden;
        timeManager.SetNewTimeSpeed(TimeManager.NewTimeType.Wall);
    }

#region DEBUG_FUNC

[System.Diagnostics.Conditional("DEBUG")]
public void ColorStatus(){
    //TODO : change la couleur du perso en fonction de son êtat
}

#endregion
}
