using System;

namespace _Project.Scripts.Core
{
    public interface IHaveHealth
    {
        event Action OnHealthChanged;
        event Action OnDeath;
        float CurrentHealth { get; }
        float MaxHealth { get; }
    }
}