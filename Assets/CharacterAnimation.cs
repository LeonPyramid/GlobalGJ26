using DG.Tweening;
using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class CharacterAnimation : Character
{
    bool flip;
    [SerializeField]float timer;
    protected override void Start()
    {
        ResetTimer();
        base.Start();
    }

    private void Update()
    {
        if(isStatic)
            LookAtRandomDirection();
    }

    private void ResetTimer()
    {
        timer = Random.Range(1, 10);
    }

    public void LookAtRandomDirection()
    {
        if (timer > 0 && !isStatic)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            LookLeft(flip);
            ResetTimer();
            flip = !flip;
        }
    }


    public void LookLeft(bool lookLeft)
    {
        head.transform.DOScaleX(lookLeft ? 1 : -1, 0.01f);
        body.transform.DOScaleX(lookLeft ? 1 : -1, 0.01f);    
    }
}

public class Character : MonoBehaviour
{
    [SerializeField] protected GameObject head;
    [SerializeField] protected GameObject body;

    [SerializeField] protected bool isStatic;

    protected virtual void Start()
    {
        head.transform.DOLocalMoveY(0.05f, 2f).SetLoops(-1, LoopType.Yoyo);
    }
}