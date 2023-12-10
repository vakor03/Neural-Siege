using System;

namespace _Project.Scripts
{
    public interface IHaveHealth
    {
        event Action OnHealthChanged;
        event Action OnDeath;
        float CurrentHealth { get; }
        float MaxHealth { get; }
    }
}