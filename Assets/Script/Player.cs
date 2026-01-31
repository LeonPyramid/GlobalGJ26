using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private float jumpDistance;
    Camera m_Camera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        m_Camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Mouse mouse = Mouse.current;
        if (mouse.leftButton.wasPressedThisFrame)
        {
            //Vector3 mousePosition = mouse.position.ReadValue();
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (mousePosition - (Vector2)transform.position).normalized;

            Debug.DrawRay(transform.position, direction * 100, Color.yellow);

            //MoveToPos(mousePosition);
            // Ray ray = m_Camera.ScreenPointToRay(mousePosition);
            // if (Physics.Raycast(ray, out RaycastHit hit))
            // {

            //     MoveToPos(hit.point);
            // }
        }
    }

    private void MoveToPos(Vector2 pos){
        transform.DOMove(pos,1).SetEase(Ease.OutSine);
    }

    private void OnMouseDown()
    {
        
    }
}
