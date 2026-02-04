
using UnityEngine;


public interface IInteractable
{
    public bool CanInteract { get; }

    public void ExecuteAction();
    public void OnPlayerEnter(Player player);
    public void OnPlayerExit();
    void SetAvailability(bool isAvailable);
}
