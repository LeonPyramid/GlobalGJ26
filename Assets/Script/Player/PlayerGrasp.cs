using UnityEngine;

public class PlayerGrasp : MonoBehaviour
{
    [SerializeField] public Player player
    {
        get;
        private set;
    }
    void Start()
    {
        player = this.GetComponentInParent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
