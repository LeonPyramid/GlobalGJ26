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
    private MaskEnum _currentMask;
    public MaskEnum CurrentMask => _currentMask;


    private bool _isGamePaused = false;
    private bool _isGameOver = false;

    public static Action<bool> OnGameOver; //true -> win | false -> loose
    public static Action OnKeyPickUp;
    public static Action<bool> OnGamePause;
    public static Action<int> OnGoldUpdated;
    public static Action<int> OnDashUpdated;

    private ExitDoor door;

    [SerializeField] public GameState gameState = GameState.PreGame;    
    void Start()
    {
        _isGameOver = false;
        AudioController.Instance.PlayAudio(levelMusic);
        QteBehaviour.Instance.OnDone += OnQteDone;
        PreGameUI.Instance.OnMaskChanged += OnMaskChanged;
        ExitDoor.OnPlayerExit += GameOver;
    }

    private void OnMaskChanged(MaskEnum maskEnum)
    {
        _currentMask = maskEnum;
    }

    public bool HasMaskEquiped(MaskEnum maskEnum)
    {
        return maskEnum == _currentMask;
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
        Bin.OnKeyFound += HasKey;
        //ExitDoor.OnPlayerExit += GameOver;
    }

    private void OnDisable()
    {
        CopGrasp.OnPlayerCatched -= GameOver;
        Bin.OnKeyFound -= HasKey;
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

    public void HasKey(){
        door = FindAnyObjectByType<ExitDoor>();
        door.UnlockDoor();
    }
}
