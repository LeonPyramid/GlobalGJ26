using UnityEngine;

public class ExtWall : MonoBehaviour
{
    public enum Orientation {Horizontal, Vertical};
    [SerializeField] public Orientation wallOrientation
    {
        get { return wallOrientation; }
    }


}
