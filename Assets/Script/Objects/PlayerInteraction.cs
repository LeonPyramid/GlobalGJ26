using UnityEngine;
using UnityEngine.InputSystem;

public class PLayerInteraction : MonoBehaviour
{

    public enum Status { Far, Close, Interacted };

    [SerializeField] public Status status;

    private SpriteRenderer spriteR;
    private InputAction interAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        status = Status.Far;
        spriteR = GetComponent<SpriteRenderer>();
        interAction = InputSystem.actions.FindAction("Attack");
    }

    // Update is called once per frame
    void Update()
    {
        if(interAction.WasPerformedThisFrame() && status == Status.Close){
            status = Status.Interacted;
        }
        ColorStatus();

    }

    void OnButtonPressed(InputControl button){

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("HasTouched");
        int CollLayer = collision.gameObject.layer;
        if(CollLayer==LayerMask.NameToLayer("PlayerRange")){
            status = Status.Close;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        int CollLayer = collision.gameObject.layer;
         if(CollLayer==LayerMask.NameToLayer("PlayerRange")){
            status = Status.Far;
        }
    }


    [System.Diagnostics.Conditional("DEBUG")]
    public void ColorStatus(){
        Color col = status switch
        {
            Status.Far => Color.white,
            Status.Close => Color.green,
            Status.Interacted => Color.red,
            _ => Color.black,
        };
        spriteR.color = col;
    }

}
