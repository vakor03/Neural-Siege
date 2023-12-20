using System;

namespace _Project.Scripts.Core
{
    public interface IHaveHealth
    {
        public event Action OnHealthChanged;
        public event Action OnDeath;
        public float CurrentHealth { get; }
        public float MaxHealth { get; }
    }
}