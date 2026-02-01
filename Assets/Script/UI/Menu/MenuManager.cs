using System;
using System.Collections.Generic;
using System.Linq;
using Utils.Singleton;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace UI.Menu
{
    public class MenuManager : Singleton<MenuManager>
    {
        [Header("Pause")]
        [SerializeField] private KeyCode pauseKey;
        [SerializeField] private PauseMenu pauseMenu;

        [Header("Positions")]
        [SerializeField] private Vector2 menuActiveHighPosition = new(0, 400);
        public Vector2 MenuActiveHighPosition => menuActiveHighPosition;
        [SerializeField] private Vector2 menuActiveLowPosition = new(0, -500);
        public Vector2 MenuActiveLowPosition => menuActiveLowPosition;
        [SerializeField] private Vector2 menuInactivePosition;
        public Vector2 MenuInactivePosition => menuInactivePosition;
        [SerializeField] private float menuOffset;
        public float MenuOffset => menuOffset;

        [Header("MainMenu")]
        [SerializeField] private Button mainMenuButton;

        
        private readonly List<MenuShown> _menus = new();
        public int MenuCount => _menus.Count;

        public Action<int> OnMenuCountChanged;

        private bool _paused;

        private GameState _stateBeforePause;

        private void Start()
        {
            mainMenuButton.onClick.AddListener(BackToMainMenu);
        }

        private void BackToMainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }

        private void Update()
        {
            if(Input.GetKeyDown(pauseKey))
            {
                if(_paused)
                {
                    GameManager.Instance.ChangeGameState(_stateBeforePause);
                    Resume();
                } 
                else
                {
                    _stateBeforePause = GameManager.Instance.gameState;
                    GameManager.Instance.ChangeGameState(GameState.Pause);
                    Pause();
                }
            }
        }

        private float _oldTime = 1f;

        private void Resume()
        {
            HideAll();

            Time.timeScale = _oldTime;

            _paused = false;
        }

        private void Pause()
        {
            _oldTime = Time.timeScale;

            Time.timeScale = 0f;

            _paused = true;

            pauseMenu.Pause();
        }

        public void AddMenuShown(Menu menu, MenuShowingState showingState = MenuShowingState.Normal)
        {
            if(_menus.Any(m => m.Menu == menu)) return;

            switch (showingState)
            {
                case MenuShowingState.Low:
                {
                    foreach (var menuToUpdate in _menus.Where(menuToUpdate => menuToUpdate.ShowingState == MenuShowingState.Normal))
                    {
                        menuToUpdate.ShowingState = MenuShowingState.High;
                    }

                    break;
                }
                case MenuShowingState.Normal when _menus.Any(m => m.ShowingState == MenuShowingState.Low):
                    showingState = MenuShowingState.Low;
                    break;
                case MenuShowingState.High when _menus.Any(m => m.ShowingState == MenuShowingState.Low):
                    foreach (var menuToHide in _menus.Where(menuToUpdate => menuToUpdate.Menu.AlwaysHigh).ToList())
                    {
                        menuToHide.Menu.Hide();
                    }
                    showingState = MenuShowingState.High;
                    break;
                case MenuShowingState.High:
                    foreach (var menuToHide in _menus.Where(menuToUpdate => menuToUpdate.Menu.AlwaysHigh).ToList())
                    {
                        menuToHide.Menu.Hide();
                    }
                    showingState = MenuShowingState.Normal;
                    break;
                default:
                    showingState = MenuShowingState.Normal;
                    break;
            }
            
            _menus.Insert(
                0, 
                new MenuShown(menu, showingState)
            );
            
            ReorderMenu();
        }

        public void RemoveMenuShown(Menu menu)
        {
            var toRemove = _menus.FirstOrDefault(m => m.Menu == menu);
            
            if (toRemove != null)
                _menus.Remove(toRemove);
            
            ReorderMenu();
        }

        public void BackTo(Menu menu)
        {
            var menusToHide = new List<MenuShown>();
            for (var i = 0; i < _menus.Count; i++)
            {
                if (_menus[i].Menu == menu)
                {
                    if (i == 0)
                    {
                        return;
                    }
                    
                    break;
                }
                menusToHide.Add(_menus[i]);
            }

            foreach (var menuToHide in menusToHide)
            {
                menuToHide.Menu.Hide();
            }
            
            ReorderMenu();
        }

        public void HideAll()
        {
            foreach (var menu in _menus.ToList())
            {
                menu.Menu.Hide();
            }
        }

        public void HideFirst()
        {
            if(MenuCount <= 0) return;
            
            _menus[0].Menu.Hide();
        }

        private void ReorderMenu()
        {
            OnMenuCountChanged?.Invoke(MenuCount);

            if (_menus.All(m => m.ShowingState != MenuShowingState.Low))
            {
                foreach (var menuShown in _menus)
                {
                    menuShown.ShowingState = MenuShowingState.Normal;
                }
            }
            
            var i = 0;
            
            var lastShowingState = MenuShowingState.Normal;
            
            foreach (var menu in _menus.ToList())
            {
                if (lastShowingState == MenuShowingState.Low && menu.ShowingState == MenuShowingState.High)
                {
                    i = _menus.Count(m => m.ShowingState == MenuShowingState.Low) - 1;
                }
                
                menu.Menu.MoveOrder(i, menu.ShowingState);
                
                i++;
                
                lastShowingState = menu.ShowingState;
            }
        }
    }

    public class MenuShown
    {
        public Menu Menu { get; }

        public MenuShowingState ShowingState { get; set; }

        public MenuShown(Menu menu, MenuShowingState showingState = MenuShowingState.Normal)
        {
            Menu = menu;
            ShowingState = showingState;
        }
    }

    public enum MenuShowingState
    {
        Normal,
        High,
        Low
    }
}