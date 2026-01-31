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

    public static Action<bool> OnGameOver; //true -> win | false -> loose
    public static Action OnKeyPickUp;
    public static Action<bool> OnGamePause;
    public static Action<int> OnGoldAdded;
    void Start()
    {
        _isGameOver = false;
        AudioController.Instance.PlayAudio(levelMusic);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            GameOver(true);
        }
        if(Input.GetKeyDown(KeyCode.L))
        {
            GameOver(false);
        }
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
        CopGrasp.OnPlayerCatched += GameOver;
        //ExitDoor.OnPlayerExit += GameOver;
    }

    private void OnDisable()
    {
        CopGrasp.OnPlayerCatched -= GameOver;
        //ExitDoor.OnPlayerExit += GameOver;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene("Game");
    }

    private void GameOver(bool isWin)
    {
        OnGameOver?.Invoke(isWin);
        _isGameOver = true;
    }

    public void SetGamePause(bool pause)
    {
        Time.timeScale = pause ? 0 : 1;
    }
}
