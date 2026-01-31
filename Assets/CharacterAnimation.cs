using DG.Tweening;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    [SerializeField] GameObject head;
    [SerializeField] GameObject body;
    void Start()
    {
        head.transform.DOMoveY(.015f, 2f).SetLoops(-1, LoopType.Yoyo);
    }

    public void LookLeft(bool lookLeft)
    {
        head.transform.DOScaleX(lookLeft ? 1 : -1, 0.01f);
        body.transform.DOScaleX(lookLeft ? 1 : -1, 0.01f);    
    }
}
