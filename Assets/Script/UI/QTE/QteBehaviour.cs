using System;
using System.Collections.Generic;
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
    [SerializeField] private Color outOfRangeColor;
    [SerializeField] private Color midOutInColor;
    [SerializeField] private Color inRangeColor;
    [SerializeField] private Color midInPerfectColor;
    [SerializeField] private Color inPerfectRangeColor;
    private Texture2D _texture;

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
    [SerializeField] private string perfectText;
    [SerializeField] private string almostPerfectText;
    [SerializeField] private string inText;
    [SerializeField] private string almostText;
    [SerializeField] private string failed;
    [SerializeField] private RectTransform scoreTransform;
    [SerializeField] private Vector3 scoreScale;
    [SerializeField] private float scoreDuration;
    [SerializeField] private Ease scoreEase;

    private float _currentDir;
    [SerializeField] private float ScoreRange;
    [SerializeField] private List<float> scoreRangesPossible;

    public Action<int> OnDone;

    private void Start()
    {
        Hide(1, true);
    }

    private void Update() 
    {
        if(Input.GetKeyDown(KeyCode.Space) && handleTween != null && handleTween.active && _shown)
        {
            handleTween.Kill();

            PlayScore(_currentDir);
        }
    }

    public void Show(float dir, float scoreRange)
    {
        score.text = "";

        //VolumeManager.Instance.LerpChromaticAberration(0, .8f, appearDuration);
        //VolumeManager.Instance.LerpVignette(0f, 0.8f, appearDuration);
        lineRectTransform.anchoredPosition = dir > 0 ? new Vector2(hideX, 0) : new Vector2(-hideX, 0);
        
        globalRectTransform.localRotation = Quaternion.Euler(new Vector3(0, 0, dir > 0 ? UnityEngine.Random.Range(0, rotationRange) : -UnityEngine.Random.Range(0, rotationRange)));

        AnchorUtils.SetAnchorPresetWithoutMoving(handleRectTransform, dir > 0 ? AnchorPreset.MiddleRight : AnchorPreset.MiddleLeft);

        handleRectTransform.anchoredPosition = Vector2.zero;

        scoreRange = FixScoreRange(scoreRange);

        ColorGauge(scoreRange);

        ScoreRange = scoreRange;

        lineRectTransform
            .DOAnchorPosX(showX, appearDuration)
            .SetEase(appearEase)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                _shown = true;

                PlayHandle(dir);
            });
    }

    private float FixScoreRange(float colorRange)
    {
        var res = 0.125f;

        foreach (var possibleRange in scoreRangesPossible)
        {
            if(colorRange > possibleRange)
            {
                res = possibleRange;

                break;
            }
        }

        return res;
    }

    private void ColorGauge(float scoreRange)
    {
        var perfectScoreRange = scoreRange/2;

        _texture = new Texture2D(16, 1)
            {
                wrapMode = TextureWrapMode.Clamp,
                name = "HueTexture"
            };

            for (var i = 0; i < _texture.width; i++)
            {
                _ = Color.white;
                Color color;
                if (i >= (_texture.width / 2) - (scoreRange * _texture.width / 2) && i <= (_texture.width / 2) + (scoreRange * _texture.width / 2))
                {
                    if (i >= (_texture.width / 2) - (perfectScoreRange * _texture.width / 2) && i <= (_texture.width / 2) + (perfectScoreRange * _texture.width / 2))
                    {
                        if(i == (_texture.width / 2) - (perfectScoreRange * _texture.width / 2) || i == (_texture.width / 2) + (perfectScoreRange * _texture.width / 2))
                        {
                            color = midInPerfectColor;
                        } 
                        else
                        {
                            color = inPerfectRangeColor;
                        }
                    }
                    else
                    {
                        if(i == (_texture.width / 2) - (scoreRange * _texture.width / 2) || i == (_texture.width / 2) + (scoreRange * _texture.width / 2))
                        {
                            color = midOutInColor;
                        } 
                        else
                        {
                            color = inRangeColor;
                        }
                    }
                }
                else
                {
                    color = outOfRangeColor;
                }

                _texture.SetPixel(i, i, color);
            }

            _texture.Apply();

            rawImage.texture = _texture;
    }

    private void PlayHandle(float dir)
    {
        AnchorUtils.SetAnchorPresetWithoutMoving(handleRectTransform, dir > 0 ? AnchorPreset.MiddleLeft : AnchorPreset.MiddleRight);
        _currentPivot = handleRectTransform.anchoredPosition.x / 2;

        handleTween = handleRectTransform
            .DOAnchorPosX(0, handleDuration / handleSpeedMultiplier)
            .SetEase(handleEase)
            .SetUpdate(true)
            .OnUpdate(() =>
            {
                _currentScore = (handleRectTransform.anchoredPosition.x - _currentPivot) / _currentPivot;
            })
            .OnComplete(() =>
            {
                Hide(dir);
            });
    }

    private void PlayScore(float dir)
    {

        var score = CalculateScore();

        scoreTransform
            .DOScale(scoreScale, scoreDuration)
            .SetEase(scoreEase)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                scoreTransform
                    .DOScale(Vector3.one, scoreDuration)
                    .SetEase(scoreEase)
                    .SetUpdate(true)
                    .OnComplete(() =>
                    {
                       Hide(dir, false, score);
                    });
            });
    }

    private int CalculateScore()
    {
        if(Mathf.Abs(_currentScore) < ScoreRange)
        {
            AudioController.Instance.PlayAudio(Audio.AudioType.SFX_Gold);
            if (Mathf.Abs(_currentScore) < ScoreRange/2)
            {
                score.text = perfectText;
                score.color = inPerfectRangeColor;

                return 2;
            } 
            else
            {
                score.text = inText;
                score.color = inRangeColor;

                return 1;
            }
        } 
        else
        {
            AudioController.Instance.PlayAudio(Audio.AudioType.SFX_Failed);
            score.text = failed;
            score.color = outOfRangeColor;

            return 0;
        }
    }

    private void Hide(float dir, bool fast = false, int score = 0)
    {
        var hideXPos = dir > 0 ? -hideX : hideX;
        //VolumeManager.Instance.LerpChromaticAberration(.8f, 0f, appearDuration);
        //VolumeManager.Instance.LerpVignette(.8f, 0f, appearDuration);
        if (fast)
        {
            lineRectTransform.position = new Vector2(hideXPos, 0f);

            _shown = false;

            return;
        }

        lineRectTransform
            .DOAnchorPosX(hideXPos, appearDuration)
            .SetEase(appearEase)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                _shown = false;

                OnDone?.Invoke(score);
            });
    }

}