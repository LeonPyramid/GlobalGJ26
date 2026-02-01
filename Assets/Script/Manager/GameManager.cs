using Audio;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Utils.Singleton;
using AudioType = Audio.AudioType;

public enum GameState { PreGame, Pause, Moving, Qte, GameOver };


public class GameManager : Utils.Singleton.Singleton<GameManager>
{
    [SerializeField] AudioType levelMusic;
    //[SerializeField] AudioType levelMusic;
    //[SerializeField] AudioType levelMusic;

    private int _dashCount;
    private int _goldAmount;
    private int _score;


    private bool _isGamePaused = false;
    private bool _isGameOver = false;

    public static Action<bool> OnGameOver; //true -> win | false -> loose
    public static Action OnKeyPickUp;
    public static Action<bool> OnGamePause;
    public static Action<int> OnGoldUpdated;
    public static Action<int> OnDashUpdated;

    [SerializeField] public GameState gameState = GameState.PreGame;
    void Start()
    {
        _isGameOver = false;
        AudioController.Instance.PlayAudio(levelMusic);
        QteBehaviour.Instance.OnDone += OnQteDone;
    }

    private void Update()
    {
        // if(Input.GetKeyDown(KeyCode.W))
        // {
        //     GameOver(true);
        // }
        // if(Input.GetKeyDown(KeyCode.L))
        // {
        //     GameOver(false);
        // }
    }

    public void ChangeGameState(GameState state){
        gameState = state;
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
        gameState = GameState.GameOver;
        Debug.Log("MG has game over");
        OnGameOver?.Invoke(isWin);
        _isGameOver = true;
    }

    private void OnQteDone(int score)
    {
        _score += score;

        OnGoldUpdated?.Invoke(_score);
    }

    public void SetGamePause(bool pause)
    {
        Time.timeScale = pause ? 0 : 1;
    }

    public void AddDash()
    {
        _dashCount++;
        
        OnDashUpdated?.Invoke(_dashCount);
    }
}
