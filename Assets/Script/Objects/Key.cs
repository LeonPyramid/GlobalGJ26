using UI.Menu;
using Audio;
using AudioType = Audio.AudioType;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class Key : MonoBehaviour
{
    public void KeyPickedUp()
    {
        GetComponent<SpriteRenderer>().enabled = true;
        Vector2 dest = Vector2.up; 
        transform.DOLocalMove(dest,1.0f).SetEase(Ease.OutQuart).OnComplete(()=>
        {
            this.gameObject.SetActive(false);
        });
        AudioController.Instance.PlayAudio(AudioType.SFX_NewBestScore);
    }
}