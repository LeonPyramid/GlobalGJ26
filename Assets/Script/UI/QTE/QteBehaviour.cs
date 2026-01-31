using Audio;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils.Anchor;
using Utils.Singleton;

public class QteBehaviour : Singleton<QteBehaviour>
{
    [Header("Global")]
    [SerializeField] private RectTransform globalRectTransform;

    [Header("Line")]
    [SerializeField] private RectTransform lineRectTransform;

    [Header("Gauge")]
    [SerializeField] private RawImage rawImage;

    [Header("Handle")]
    [SerializeField] private RectTransform handleRectTransform;

    [Header("Show")]
    [SerializeField] private float rotationRange;
    [SerializeField] private float showX;
    [SerializeField] private float appearDuration;
    [SerializeField] private Ease appearEase;

    [Header("Handle")]
    [SerializeField] private float handleDuration;
    [SerializeField] private Ease handleEase;
    [SerializeField] private float handleSpeedMultiplier = 1;
    private Tween handleTween;
    private float _currentPivot;
    private float _currentScore;

    [Header("Hide")]
    [SerializeField] private float hideX;
    [SerializeField] private float hideDuration;
    [SerializeField] private Ease hideEase;
    private bool _shown;

    [Header("Score")]
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private RectTransform scoreTransform;
    [SerializeField] private Vector3 scoreScale;
    [SerializeField] private float scoreDuration;
    [SerializeField] private Ease scoreEase;

    private float _currentDir;

    private void Start()
    {
        Hide(1, true);
    }

    private void Update() 
    {
        if(Input.GetKeyDown(KeyCode.Space) && handleTween != null && handleTween.active)
        {
            handleTween.Kill();

            PlayScore(_currentDir);
        }
    }

    public void Show(float dir, float scoreRange)
    {
        score.text = "";

        lineRectTransform.anchoredPosition = dir > 0 ? new Vector2(hideX, 0) : new Vector2(-hideX, 0);
        
        globalRectTransform.localRotation = Quaternion.Euler(new Vector3(0, 0, dir > 0 ? Random.Range(0, rotationRange) : -Random.Range(0, rotationRange)));

        AnchorUtils.SetAnchorPresetWithoutMoving(handleRectTransform, dir > 0 ? AnchorPreset.MiddleRight : AnchorPreset.MiddleLeft);

        handleRectTransform.anchoredPosition = Vector2.zero;

        lineRectTransform
            .DOAnchorPosX(showX, appearDuration)
            .SetEase(appearEase)
            .OnComplete(() =>
            {
                _shown = true;

                PlayHandle(dir);
            });
    }

    private void PlayHandle(float dir)
    {
        AnchorUtils.SetAnchorPresetWithoutMoving(handleRectTransform, dir > 0 ? AnchorPreset.MiddleLeft : AnchorPreset.MiddleRight);

        _currentPivot = handleRectTransform.anchoredPosition.x / 2;

        handleTween = handleRectTransform
            .DOAnchorPosX(0, handleDuration / handleSpeedMultiplier)
            .SetEase(handleEase)
            .OnUpdate(() =>
            {
                _currentScore = (handleRectTransform.anchoredPosition.x - _currentPivot) / _currentPivot;

                score.text = _currentScore.ToString();
            })
            .OnComplete(() =>
            {
                Hide(dir);
            });
    }

    private void PlayScore(float dir)
    {
        AudioController.Instance.PlayAudio(Audio.AudioType.SFX_NewBestScore);

        scoreTransform
            .DOScale(scoreScale, scoreDuration)
            .SetEase(scoreEase)
            .OnComplete(() =>
            {
                scoreTransform
                    .DOScale(Vector3.one, scoreDuration)
                    .SetEase(scoreEase)
                    .OnComplete(() =>
                    {
                       Hide(dir);
                    });
            });
    }

    private void Hide(float dir, bool fast = false)
    {
        var hideXPos = dir > 0 ? -hideX : hideX;

        if(fast)
        {
            lineRectTransform.position = new Vector2(hideXPos, 0f);

            _shown = false;

            return;
        }

        lineRectTransform
            .DOAnchorPosX(hideXPos, appearDuration)
            .SetEase(appearEase)
            .OnComplete(() =>
            {
                _shown = false;
            });
    }

}