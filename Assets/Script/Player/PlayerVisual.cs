using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    [Header("Hide")]
    [SerializeField] private List<SpriteRenderer> spriteRenderers;
    [SerializeField] private float hideOpacity;
    [SerializeField] private float visibleOpacity;
    [SerializeField] private float hideDuration;

    private void Start()
    {
        HideMode(false);
    }

    public void Turn(float dir)
    {
        if(dir > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        } 
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public void HideMode(bool hideMode)
    {
        float opacity = hideMode? hideOpacity : visibleOpacity;

        foreach(var spriteRenderer in spriteRenderers)
        {
            spriteRenderer.DOFade(opacity, hideDuration);
        }
    }
}