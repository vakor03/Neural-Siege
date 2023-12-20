﻿using System;
using UnityEngine;

namespace _Project.Scripts.Core
{
    public interface IPlayerBase
    {
        int MaxHealth { get; }
        int CurrentHealth { get; }
        event Action OnHealthChanged;
        event Action OnDeath;
        void TakeDamage(int damage);
    }

    public class PlayerBase : IPlayerBase
    {
        public PlayerBase(int maxHealth)
        {
            MaxHealth = maxHealth;
            CurrentHealth = maxHealth;
        }

        public int MaxHealth { get; private set; }
        public int CurrentHealth { get; private set; }
        public event Action OnHealthChanged;
        public event Action OnDeath;

        public void TakeDamage(int damage)
        {
            CurrentHealth -= damage;
            OnHealthChanged?.Invoke();

            if (CurrentHealth <= 0)
            {
                OnDeath?.Invoke();
            }
        }
    }
}