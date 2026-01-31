using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Menu
{
    public class MenuBackgroundHider: MonoBehaviour
    {
        [SerializeField] private Image backgroundHider;
        [SerializeField] private float duration;
        [SerializeField] private float startOpacity;
        [SerializeField] private float endOpacity;

        private void Start()
        {
            MenuManager.Instance.OnMenuCountChanged += OnMenuCountChanged;
            
            backgroundHider
                .DOFade(startOpacity, duration)
                .OnComplete(() =>
                {
                    backgroundHider.enabled = false;
                });
        }

        private void OnMenuCountChanged(int obj)
        {
            switch (obj)
            {
                case > 0:
                    backgroundHider.enabled = true;
                    backgroundHider.DOKill();
                    backgroundHider
                        .DOFade(endOpacity, duration);
                    break;
                case 0:
                    backgroundHider.DOKill();
                    backgroundHider
                        .DOFade(startOpacity, duration)
                        .OnComplete(() =>
                        {
                            backgroundHider.enabled = false;
                        });
                    break;
            }
        }
    }
}