using System;
using System.Collections.Generic;
using System.Text;

namespace Assets.Script.Interfaces
{
    public interface IInteractable
    {
        void Interact(Player player);
        float Cooldown { get; }
        bool IsActive { get; }
    }
}
