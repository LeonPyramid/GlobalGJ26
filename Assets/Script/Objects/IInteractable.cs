public  interface IInteractable
{
    public interface IInteractable
    {
        void Interact(Player player);
        float Cooldown { get; }
        bool IsActive { get; }
    }
}