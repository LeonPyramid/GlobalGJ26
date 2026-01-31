using DG.Tweening;
using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class CharacterAnimation : MonoBehaviour
{
    [SerializeField] GameObject head;
    [SerializeField] GameObject body;

    [SerializeField] bool isStatic;

    bool flip;
    [SerializeField]float timer;
    void Start()
    {
        ResetTimer();
        head.transform.DOLocalMoveY(0.08f, 2f).SetLoops(-1, LoopType.Yoyo);
    }

    private void Update()
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

    private void ResetTimer()
    {
        timer = Random.Range(1, 10);
    }

    public void LookLeft(bool lookLeft)
    {
        head.transform.DOScaleX(lookLeft ? 1 : -1, 0.01f);
        body.transform.DOScaleX(lookLeft ? 1 : -1, 0.01f);    
    }
}
