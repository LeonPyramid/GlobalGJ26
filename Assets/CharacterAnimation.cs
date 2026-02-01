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

    [SerializeField] List<Color> habitsColors;

    [SerializeField] List<Color> skinColors;

    [SerializeField] List<Sprite> Hats;

    [SerializeField] List<Sprite> Accessories;

    [SerializeField] SpriteRenderer hatRenderer;

    [SerializeField] SpriteRenderer BodyRenderer;
    [SerializeField] SpriteRenderer HeadRenderer;

    [SerializeField] SpriteRenderer accessoryRenderer;

    protected override void Start()
    {
        //Randomize Objects

        if(Random.Range(0,2)==0){
            hatRenderer.sprite = Hats[Random.Range(0, Hats.Count)];
            SetRandomColor(hatRenderer, habitsColors);
        }
        else{
            hatRenderer.enabled = false;
        }

        SetRandomColor(HeadRenderer, skinColors);
        SetRandomColor(BodyRenderer, habitsColors);

        if(Random.Range(0,3)==0){
            accessoryRenderer.sprite = Accessories[Random.Range(0, Accessories.Count)];
            SetRandomColor(accessoryRenderer, habitsColors);
        }
        else{
            accessoryRenderer.enabled = false;
        }
        ResetTimer();

        head.transform.DOMoveY(.015f, 2f).SetLoops(-1, LoopType.Yoyo);
        base.Start();
    }

    void SetRandomColor(SpriteRenderer rend, List<Color> colList){
        rend.color = colList[Random.Range(0, colList.Count)];
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
        if (timer > 0)
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
        head.transform.DOScaleX(lookLeft ? 1 : -1, 0.01f).SetUpdate(true);
        body.transform.DOScaleX(lookLeft ? 1 : -1, 0.01f).SetUpdate(true);    
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