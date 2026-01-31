using UI.Menu;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Menu menu;

    public void Pause()
    {
        menu.Show();
    }
}