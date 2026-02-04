using Audio;
using DG.Tweening;
using Script.UI.BinUI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Bin : MonoBehaviour, IInteractable
{
    [Header("State")]
    [SerializeField] private bool hasKey = false;
    private bool isSearched = false;
    private bool isPlayerInside = false;

    [Header("Settings")]
    [SerializeField] private float useCoolDown = 10f;
    [SerializeField] private float ejectCoolDown = 3f;

    [Header("Visuals")]
    [SerializeField] private SpriteRenderer mainRenderer;
    [SerializeField] private List<Sprite> emptySprites;
    [SerializeField] private List<Sprite> fullSprites;

    [Header("Feedback")]
    [SerializeField] private BinUI binUI;
    [SerializeField] private float shakeDuration = 0.5f;

    private bool isUsable = true;
    private bool playerInside = false;
    private Player currentPlayer;

    public bool IsActive => isUsable;
    public float Cooldown => useCoolDown;

    public static event Action OnKeyFound;

    public void Interact(Player player)
    {
        if (isPlayerInside) return;

        EnterBin(player);

        if (hasKey && !isSearched)
        {
            Debug.Log("Clé trouvée !");
            isSearched = true;
            hasKey = false;
            OnKeyFound?.Invoke();
        }
    }

    private void EnterBin(Player player)
    {
        playerInside = true;
        currentPlayer = player;

        player.SetHidden();
        player.transform.position = transform.position;
        player.OnClick += LeaveBin; 

        ApplyVisual(fullSprites);
        PlayShake();

        if (binUI) binUI.PlayFill();


        StartCoroutine(EjectTimer());
    }

    private void LeaveBin()
    {
        if (!playerInside) return;

        StopAllCoroutines();
        playerInside = false;

        currentPlayer.OnClick -= LeaveBin;
        currentPlayer.SetStatic();

        PlayShake();
        if (binUI) binUI.StopFill();

        StartCoroutine(CooldownRoutine());
    }

    private IEnumerator EjectTimer()
    {
        yield return new WaitForSecondsRealtime(ejectCoolDown);
        if (playerInside)
        {
            Debug.Log("Temps écoulé ! Éjection automatique.");
            LeaveBin();
        }
    }

    private IEnumerator CooldownRoutine()
    {
        isUsable = false;
        ApplyVisual(emptySprites);
        yield return new WaitForSeconds(useCoolDown);
        isUsable = true;
    }

    private void ApplyVisual(List<Sprite> pool)
    {
        if (pool != null && pool.Count > 0)
            mainRenderer.sprite = pool[Random.Range(0, pool.Count)];
    }

    private void PlayShake()
    {
        AudioController.Instance?.PlayAudio(Audio.AudioType.SFX_Bin, false, 0f, Random.Range(.8f, 1f));
        transform.DOShakeRotation(shakeDuration, new Vector3(0, 0, 10)).SetUpdate(true);
    }

    // Méthode pour configurer la poubelle via un GameManager
    public void AssignKey() => hasKey = true;
}