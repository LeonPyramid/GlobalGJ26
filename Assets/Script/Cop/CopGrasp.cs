using Unity.VisualScripting;
using UnityEngine;

public class CopGrasp : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        int CollLayer = collision.gameObject.layer;
        if(CollLayer==LayerMask.NameToLayer("PlayerBody")){
            Player player = collision.gameObject.GetComponent<Player>();
            if(player.status != Player.Status.Hidden){
                Debug.Log("Player is dead..");
            }
        }
    }

}
