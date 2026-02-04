using DG.Tweening;
using UnityEngine;

namespace Script.UI.BinUI
{
    public class BinUI : MonoBehaviour
    {
        [Header("Bar")]
        [SerializeField] private RectTransform barTransform;

        [Header("Fill")]
        [SerializeField] private RectTransform fillTransform;
        [SerializeField] private float fillStartX;
        [SerializeField] private float fillEndX;

        [Header("Vibrato")]
        [SerializeField] private int vibratoMin;
        [SerializeField] private int vibratoMax;
        [SerializeField] private float shakeStrengthMin;
        [SerializeField] private float shakeStrengthMax;
        [SerializeField] private float shakeDuration = 0.15f;

        private Tween _fillTween;
        private Tween _shakeTween;
        private Tween _shakeControlTween;

        private Vector2 _barInitialPos;
        private float _currentStrength;
        private int _currentVibrato;

        private void Start()
        {
            _barInitialPos = barTransform.anchoredPosition;
            barTransform.gameObject.SetActive(false);
        }

        public void PlayFill(float fillDuration)
        {
            barTransform.gameObject.SetActive(true);

            _fillTween?.Kill();
            _shakeTween?.Kill();
            _shakeControlTween?.Kill();

            barTransform.anchoredPosition = _barInitialPos;
            fillTransform.anchoredPosition =
                new Vector2(fillStartX, fillTransform.anchoredPosition.y);

            _currentStrength = shakeStrengthMin;
            _currentVibrato = vibratoMin;

            _fillTween = fillTransform
                .DOAnchorPosX(fillEndX, fillDuration)
                .SetEase(Ease.Linear)
                .SetUpdate(true);

            StartShake();

            _shakeControlTween = DOTween.To(
                () => 0f,
                t =>
                {
                    _currentStrength = Mathf.Lerp(shakeStrengthMin, shakeStrengthMax, t);
                    _currentVibrato = Mathf.RoundToInt(Mathf.Lerp(vibratoMin, vibratoMax, t));
                },
                1f,
                fillDuration
            )
            .SetEase(Ease.Linear)
            .SetUpdate(true);
        }

        private void StartShake()
        {
            _shakeTween?.Kill();

            _shakeTween = barTransform
                .DOShakeAnchorPos(
                    shakeDuration,
                    new Vector2(_currentStrength, _currentStrength),
                    _currentVibrato,
                    90,
                    false,
                    true
                )
                .SetUpdate(true)
                .OnComplete(StartShake);
        }

        public void StopFill(float unfillDuration)
        {
            _fillTween?.Complete();
            _shakeTween?.Kill();
            _shakeControlTween?.Kill();
            PlayUnfill(unfillDuration);
        }

        private void PlayUnfill(float unfillDuration)
        {
            barTransform.DOKill();
            barTransform.anchoredPosition = _barInitialPos;

           _fillTween = fillTransform
                .DOAnchorPosX(fillStartX, unfillDuration)
                .SetEase(Ease.Linear)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    barTransform.gameObject.SetActive(false);
                });
        }
    }
}
