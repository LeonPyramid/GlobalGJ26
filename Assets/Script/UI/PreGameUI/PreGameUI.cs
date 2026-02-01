using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Utils.Singleton;


public class PreGameUI : Singleton<PreGameUI>
{
    [SerializeField] List<MaskTemplate> maskTemplates;
    [SerializeField] private Button playButton;

    [Header("Hide")]
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private float hideDuration;
    [SerializeField] private float hidePosY;

    private MaskTemplate _currentMaskSelected;

    public Action<MaskEnum> OnMaskChanged;

    public Action OnGameStarted;
    
    private void Start()
    {
        foreach (var mask in maskTemplates)
        {
            mask.OnHovered += OnMaskHovered;

            mask.OnSelected += OnMaskSelected;
        }

        foreach (var mask in maskTemplates)
        {
            if(mask.Mask == MaskEnum.Basic)
            {
                mask.Select();

                break;
            }
        }

        playButton.onClick.AddListener(Hide);
    }

    private void OnMaskHovered(MaskTemplate mask)
    {
        foreach (var maskTemp in maskTemplates)
        {
            if(mask == maskTemp) continue;

            maskTemp.StopHover();
        }
    }

    private void OnMaskSelected(MaskTemplate mask)
    {
        _currentMaskSelected = mask;

        foreach (var maskTemp in maskTemplates)
        {
            if(mask == maskTemp) continue;

            maskTemp.Unselect();
        }

        OnMaskChanged?.Invoke(mask.Mask);
    }

    private void Hide()
    {
        OnMaskChanged?.Invoke(_currentMaskSelected.Mask);

        rectTransform
            .DOAnchorPosY(hidePosY, hideDuration)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                GameManager.Instance.ChangeGameState(GameState.Moving);
                OnGameStarted?.Invoke();
            });
    }

    public void ForceNewMask(MaskEnum maskEnum)
    {
        OnMaskChanged?.Invoke(maskEnum);
    }
}