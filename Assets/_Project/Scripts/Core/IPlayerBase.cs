using System;

namespace _Project.Scripts.Core
{
    public interface IPlayerBase
    {
        int MaxHealth { get; }
        int CurrentHealth { get; }
        event Action OnHealthChanged;
        event Action OnDeath;
        void TakeDamage(int damage);
        void Reset();
    }
}