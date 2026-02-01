using DG.Tweening;
using TMPro;
using UnityEngine;

public class InGameUi : MonoBehaviour
{
    [Header("Timer")]
    [SerializeField] private TextMeshProUGUI timer;
    private float _startTime;
    private float _currentTime;

    [Header("Dash")]
    [SerializeField] private TextMeshProUGUI dash;
    [SerializeField] private string dashPrefix;
    [SerializeField] private Vector3 dashScale;
    [SerializeField] private float dashDuration;
    [SerializeField] private Ease dashEase;
    private int _dash;

    [Header("Score")]
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private string scorePrefix;
    [SerializeField] private RectTransform scoreTransform;
    [SerializeField] private Vector3 scoreScale;
    [SerializeField] private float scoreDuration;
    [SerializeField] private Ease scoreEase;

    private void OnEnable()
    {
        GameManager.OnGoldUpdated += OnGoldUpdated;
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

   void StartTimer()
    {
        _startTime = Time.realtimeSinceStartup;
    }

    void Update()
    {
        float elapsedTime = Time.realtimeSinceStartup - _startTime;
        float remainingTime = originalTime - elapsedTime;
        timer.text = 
    }
}