using System;
using UnityEngine;
using UnityEngine.InputSystem;






public class PlayerInteraction : MonoBehaviour
{
    [Serializable]
    public abstract class PlayerAction : MonoBehaviour {
        abstract public void ActionEffect(Collider2D ?playerCollider);
        abstract public void TimerEffect(Collider2D ?playerCollider);
        abstract public void TimerRevert();
        abstract public void SetUnblocked();
        abstract public void SetBlocked();

    }



    public enum Status { Far, Close, Interacted , Blocked};

    [SerializeField] public Status status;

    private SpriteRenderer spriteR;
    private InputAction interAction;

    private Collider2D closeCollider;

    //[SerializeField] private bool IsInteractable;

    [SerializeReference] private PlayerAction action;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteR = GetComponent<SpriteRenderer>();
        interAction = InputSystem.actions.FindAction("Attack");
        closeCollider = null;
    }

    // Update is called once per frame
    void Update()
    {
        if(interAction.WasPerformedThisFrame() && status == Status.Close){
            status = Status.Interacted;
            action.ActionEffect(closeCollider);
        }
        ColorStatus();

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(status!=Status.Blocked){
            Debug.Log("HasTouched");
            action.TimerEffect(collision);
            int CollLayer = collision.gameObject.layer;
            if(CollLayer==LayerMask.NameToLayer("PlayerRange")){
                status = Status.Close;
                closeCollider = collision;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (status != Status.Blocked)
        {
            action.TimerRevert();

            int CollLayer = collision.gameObject.layer;
            if (CollLayer == LayerMask.NameToLayer("PlayerRange"))
            {
                status = Status.Far;
                closeCollider = null;
            }
        }
    }

    public void SetBLocked(){
        status = Status.Blocked;
        action.SetBlocked();

    }

    public void SetUnblocked(){
        status = Status.Far;
        action.SetUnblocked();

    }


    [System.Diagnostics.Conditional("DEBUG")]
    public void ColorStatus(){
        // Color col = status switch
        // {
        //     Status.Far => Color.white,
        //     Status.Close => Color.green,
        //     Status.Interacted => Color.red,
        //     Status.Blocked => Color.gray,
        //     _ => Color.black,
        // };
        // spriteR.color = col;
    }


}
