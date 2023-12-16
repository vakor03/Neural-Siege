using System;
using UnityEngine;

namespace _Project.Scripts.Core
{
    public class Shop : MonoBehaviour
    {
        public static Shop Instance { get; private set; }
        [SerializeField] private int startingMoneyAmount;

        public int MoneyAmount { get; private set; }

        public event Action OnMoneyAmountChanged;

        private void Awake()
        {
            Instance = this;
            MoneyAmount = startingMoneyAmount;
        }

        public void SpendMoney(int amount)
        {
            MoneyAmount -= amount;
            OnMoneyAmountChanged?.Invoke();
        }
        
        public void EarnMoney(int amount)
        {
            MoneyAmount += amount;
            OnMoneyAmountChanged?.Invoke();
        }
    }
}