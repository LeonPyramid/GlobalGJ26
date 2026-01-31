using Audio;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Utils.Singleton;
using AudioType = Audio.AudioType;

public class GameManager : Utils.Singleton.Singleton<GameManager>
{
    [SerializeField] AudioType levelMusic;
    //[SerializeField] AudioType levelMusic;
    //[SerializeField] AudioType levelMusic;


    private int _goldAmount;
    private int _score;

    private bool _isGamePaused = false;
    private bool _isGameOver = false;

    public static Action OnGameOver;
    public static Action<bool> OnGamePause;
    public static Action<int> OnGoldAdded;
    void Start()
    {
        AudioController.Instance.PlayAudio(levelMusic);
    }

    public void OnGamePauseHandler(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _isGamePaused = !_isGamePaused;
            SetGamePause(_isGamePaused);
            OnGamePause?.Invoke(_isGamePaused);
        }
    }

    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }

    public void RestartLevel()
    {
        SceneManager.LoadScene("Game");
    }

    private void GameOver()
    {
        OnGameOver?.Invoke();
        _isGameOver = true;
    }

    public void SetGamePause(bool pause)
    {
        Time.timeScale = pause ? 0 : 1;
    }
}
