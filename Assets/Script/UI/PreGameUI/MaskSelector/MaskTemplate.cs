using System;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MaskTemplate : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private MaskEnum mask;
    public MaskEnum Mask => mask;

    [Header("ScaleMask")]
    [SerializeField] private RectTransform maskRectTransform;
    [SerializeField] private Vector3 maskScale;

    [Header("FadeIn")]
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private Image frame;
    [SerializeField] private Image bg;
    [SerializeField] private float fadeInDuration;

    [Header("FadeOut")]
    [SerializeField] private float fadeOutDuration;

    [Header("Select")]
    [SerializeField] private TextMeshProUGUI maskName;
    [SerializeField] private Color nameSelectedColor;
    [SerializeField] private Color nameUnselectedColor;
    [SerializeField] private Image maskBackground;
    [SerializeField] private Color bgSelectedColor;
    [SerializeField] private Color bgUnselectedColor;
    [SerializeField] private Image maskFrame;
    [SerializeField] private Color frameSelectedColor;
    [SerializeField] private Color frameUnselectedColor;
    [SerializeField] private float selectDuration;

    public Action<MaskTemplate> OnHovered;
    public Action<MaskTemplate> OnSelected;

    private void Start()
    {
        FadeOut(true);

        Unselect();

        button.onClick.AddListener(Select);
    }

    public void Hover()
    {
        FadeIn();

        OnHovered?.Invoke(this);
    }

    private void FadeIn()
    {
        frame.DOFade(1f, fadeInDuration).SetUpdate(true);

        bg.DOFade(1f, fadeInDuration).SetUpdate(true);

        description.DOFade(1f, fadeInDuration).SetUpdate(true);

        maskRectTransform.DOScale(maskScale, fadeInDuration).SetUpdate(true);
    }

    public void StopHover()
    {
        FadeOut();
    }

    private void FadeOut(bool fast = false)
    {
        if(fast)
        {
            frame.DOFade(0f, 0f).SetUpdate(true);

            bg.DOFade(0f, 0f).SetUpdate(true);

            description.DOFade(0f, 0f).SetUpdate(true);

            maskRectTransform.DOScale(Vector3.one, 0f).SetUpdate(true);

            return;
        }

        frame.DOFade(0f, fadeOutDuration).SetUpdate(true);

        bg.DOFade(0f, fadeOutDuration).SetUpdate(true);

        description.DOFade(0f, fadeOutDuration).SetUpdate(true);

        maskRectTransform.DOScale(Vector3.one, fadeOutDuration).SetUpdate(true);
    }

    public void Select()
    {
        maskName.DOColor(nameSelectedColor, selectDuration).SetUpdate(true);

        maskFrame.DOColor(frameSelectedColor, selectDuration).SetUpdate(true);

        maskBackground.DOColor(bgSelectedColor, selectDuration).SetUpdate(true);

        OnSelected?.Invoke(this);
    }

    public void Unselect(bool fast = false)
    {
        if(fast)
        {
            maskName.DOColor(nameUnselectedColor, 0f).SetUpdate(true);

            maskFrame.DOColor(frameUnselectedColor, 0f).SetUpdate(true);

            maskBackground.DOColor(bgUnselectedColor, 0f).SetUpdate(true);
        }

        maskName.DOColor(nameUnselectedColor, selectDuration).SetUpdate(true);

        maskFrame.DOColor(frameUnselectedColor, selectDuration).SetUpdate(true);

        maskBackground.DOColor(bgUnselectedColor, selectDuration).SetUpdate(true);
    }
}