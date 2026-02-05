using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Utils.Anchor;
using Utils.Singleton;

public class InGameUI : Singleton<InGameUI>
{
    [Header("Timer")]
    [SerializeField] private TextMeshProUGUI timer;
    public string TimerText => timer.text;

    public float ElapsedTime { get; private set; }

    private Tween _timerTween;

    [Header("Dash")]
    [SerializeField] private TextMeshProUGUI dash;
    [SerializeField] private string dashPrefix;
    [SerializeField] private RectTransform dashRect;
    [SerializeField] private Vector3 dashScale;
    [SerializeField] private float dashDuration;
    [SerializeField] private Ease dashEase;
    private int _dash;
    public string DashText => dash.text;

    [Header("Score")]
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private string scorePrefix;
    [SerializeField] private RectTransform scoreTransform;
    [SerializeField] private Vector3 scoreScale;
    [SerializeField] private float scoreDuration;
    [SerializeField] private Ease scoreEase;
    public string ScoreText => score.text;

    private void Start()
    {
        GameManager.OnGameOver += OnGameOver;
        GameManager.OnGoldUpdated += OnGoldUpdated;

        GameManager.OnDashUpdated += OnDashUpdated;
        PreGameUI.OnGameStarted += OnGameStarted;
    }

    private void OnGameOver(bool obj)
    {
        PauseTimer();
    }

    private void OnGameStarted()
    {
        StartTimer();
    }

    private void OnDestroy()
    {
        GameManager.OnGameOver -= OnGameOver;
        GameManager.OnGoldUpdated -= OnGoldUpdated;

        GameManager.OnDashUpdated -= OnDashUpdated;
        PreGameUI.OnGameStarted -= OnGameStarted;
    }

    private void OnDashUpdated(int obj)
    {
        dash.text = dashPrefix + obj;

        dashRect
            .DOScale(dashScale, dashDuration)
            .SetEase(dashEase)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                dashRect
                    .DOScale(Vector3.one, dashDuration)
                    .SetEase(dashEase)
                    .SetUpdate(true);
            });
    }

    private void OnDisable()
    {
        GameManager.OnGoldUpdated -= OnGoldUpdated;
    }

    private void OnGoldUpdated(int newScore)
    {
        score.text = scorePrefix + newScore;

        scoreTransform
            .DOScale(scoreScale, scoreDuration)
            .SetEase(scoreEase)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                scoreTransform
                    .DOScale(Vector3.one, scoreDuration)
                    .SetEase(scoreEase)
                    .SetUpdate(true);
            });
    }

    private void Update()
    {
        UpdateTimerText();
    }

    private void StartTimer()
    {
        _timerTween = DOTween.To(
                () => ElapsedTime,
                x => ElapsedTime = x,
                float.MaxValue,
                float.MaxValue
            )
            .SetEase(Ease.Linear)
            .SetUpdate(true)
            .OnUpdate(UpdateTimerText);
    }

    private void UpdateTimerText()
    {
        timer.text = TimeUtils.FormatTime(ElapsedTime);
    }

    public void PauseTimer()
    {
        _timerTween?.Pause();
    }
}
