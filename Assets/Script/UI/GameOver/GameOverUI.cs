using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;

    [Header("Show")]
    [SerializeField] private float showDuration;
    [SerializeField] private Ease showEase;

    [Header("Title")]
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private Color winColor;
    [SerializeField] private string winText;
    [SerializeField] private Color loseColor;
    [SerializeField] private string loseText;

    [Header("Scores")]
    [SerializeField] private float showScoreDuration;
    [SerializeField] private Ease showScoreEase;
    [SerializeField] private RectTransform dash;
    [SerializeField] private TextMeshProUGUI dashText;
    [SerializeField] private RectTransform score;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private RectTransform time;
    [SerializeField] private TextMeshProUGUI timeText;

    [Header("Retry")]
    [SerializeField] private float showRetryY;
    [SerializeField] private float showRetryDuration;
    [SerializeField] private Ease showRetryEase;
    [SerializeField] private RectTransform retry;
    [SerializeField] private Button retryButton;
    [SerializeField] private Button nextButton;


    private void Start()
    {
        GameManager.OnGameOver += OnGameOver;

        retryButton.onClick.AddListener(() =>
        {
            GameManager.Instance.RestartLevel();
        });

        nextButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("Game 2");
        });
    }

    private void OnGameOver(bool win)
    {
        if (win)
        {
            title.color = winColor;
            title.text = winText;
        }
        else
        {
            title.color = loseColor;
            title.text = loseText;
        }

        Show(win);
    }

    private void Show(bool win)
    {
        timeText.text = InGameUI.Instance.TimerText;
        dashText.text = InGameUI.Instance.DashText;
        scoreText.text = InGameUI.Instance.ScoreText;
        nextButton.gameObject.SetActive(win);

        rectTransform
            .DOAnchorPosY(0f, showDuration)
            .SetEase(showEase)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                if (win)
                {
                    ShowScores();
                }
                else
                {
                    ShowRetryButton();
                }
            });
    }

    private void ShowScores()
    {
        dash.DOAnchorPosX(0f, showScoreDuration)
            .SetEase(showScoreEase)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                score.DOAnchorPosX(0f, showScoreDuration)
                    .SetEase(showScoreEase)
                    .SetUpdate(true)
                    .OnComplete(() =>
                    {
                        time.DOAnchorPosX(0f, showScoreDuration)
                            .SetEase(showScoreEase)
                            .SetUpdate(true)
                            .OnComplete(() =>
                            {
                                ShowRetryButton();
                            });
                    });
            });
    }

    private void ShowRetryButton()
    {
        retry.DOAnchorPosY(showRetryY, showRetryDuration)
            .SetUpdate(true)
            .SetEase(showRetryEase);
    }

    private void OnDestroy()
    {
        GameManager.OnGameOver -= OnGameOver;
    }
}