using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class InteractationBehaviour : MonoBehaviour
{
    public enum Status { Far, Close, Interacted, Blocked };
    [SerializeField] public Status status;

    private IInteractable interactable;
    private InputAction interactAction;
    private Player currentPlayer;

    void Awake()
    {
        interactable = GetComponent<IInteractable>();
        interactAction = InputSystem.actions.FindAction("Interact");
    }

    void Update()
    {
        if ( status == Status.Close && interactAction.WasPerformedThisFrame())
        {
            if (interactable != null && interactable.CanInteract)
                interactable.ExecuteAction();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (status == Status.Blocked) return;

        if (collision.CompareTag("PlayerRange"))
        {
            SetStatus(Status.Close);
            currentPlayer = collision.GetComponentInParent<Player>();
            interactable.OnPlayerEnter(currentPlayer);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (status == Status.Blocked) return;

        if (collision.CompareTag("PlayerRange"))
        {
            SetStatus(Status.Far);
            interactable.OnPlayerExit();
            currentPlayer = null;
        }
    }

    public void SetStatus(Status newStatus) => status = newStatus;


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
