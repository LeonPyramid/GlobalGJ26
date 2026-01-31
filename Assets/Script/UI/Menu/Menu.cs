using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Menu
{
    public class Menu : MonoBehaviour
    {
        [Header("Buttons")] 
        [SerializeField] private List<Button> showButtons;
        [SerializeField] private List<Button> hideButtons;
        [SerializeField] private Button titleButtons;
        [Header("Position")]
        [SerializeField] private RectTransform rectTransform;

        [Header("Height")] 
        [SerializeField] private bool alwaysHigh;
        public bool AlwaysHigh => alwaysHigh;

        public Action OnShow;
        public Action OnHide;
        
        private void Awake()
        {
            foreach (var showButton in showButtons) 
                showButton.onClick.AddListener(Show);
            
            foreach (var hideButton in hideButtons)
                hideButton.onClick.AddListener(Hide);
            
            titleButtons.onClick.AddListener(BackTo);
        }

        bool _firstUpdate = true;
        
        private void Update()
        {
            if (!_firstUpdate) return;
            
            HideFast();
                
            _firstUpdate = false;
        }

        public void Show()
        {
            if (alwaysHigh)
            {
                ShowHigh();
                
                return;
            }
            
            gameObject.SetActive(true);

            MenuManager.Instance.AddMenuShown(this);
            
            OnShow?.Invoke();
        }

        public void ShowLow()
        {
            gameObject.SetActive(true);
            
            MenuManager.Instance.AddMenuShown(this, MenuShowingState.Low);
            
            OnShow?.Invoke();
        }

        public void ShowHigh()
        {
            gameObject.SetActive(true);
            
            MenuManager.Instance.AddMenuShown(this, MenuShowingState.High);
            
            OnShow?.Invoke();
        }

        public void Hide()
        {
            MenuManager.Instance.RemoveMenuShown(this);
            
            rectTransform.DOKill();
            
            rectTransform.DOAnchorPos(MenuManager.Instance.MenuInactivePosition, 0.15f)
                .SetEase(Ease.Linear)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    gameObject.SetActive(false);
                    OnHide?.Invoke();
                });
        }

        private void BackTo()
        {
            MenuManager.Instance.BackTo(this);
        }
        
        public void HideFast()
        {
            rectTransform.anchoredPosition = MenuManager.Instance.MenuInactivePosition;
            gameObject.SetActive(false);
        }

        public void MoveOrder(int index, MenuShowingState state = MenuShowingState.Normal)
        {
            var targetPosition = state switch
            {
                MenuShowingState.High => MenuManager.Instance.MenuActiveHighPosition,
                MenuShowingState.Low => MenuManager.Instance.MenuActiveLowPosition,
                _ => Vector2.zero
            };

            var newPosition = new Vector2(targetPosition.x, targetPosition.y + index * MenuManager.Instance.MenuOffset);
            
            transform.SetAsFirstSibling();
            
            rectTransform.DOKill();
            
            rectTransform.DOAnchorPos(newPosition, 0.15f)
                .SetEase(Ease.Linear)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    rectTransform
                        .DOShakeAnchorPos(0.3f, strength: new Vector2(10, 0f), vibrato: 1)
                        .SetEase(Ease.OutQuad)
                        .SetUpdate(true);
                });
        }
    }
}