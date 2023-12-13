using System;
using _Project.Scripts.Core.GridSystem;
using UnityEngine;

namespace _Project.Scripts.Core
{
    public class Shop : MonoBehaviour
    {
        public static Shop Instance { get; private set; }
        [SerializeField] private PlacingObjectSO[] options;
        [SerializeField] private int startingMoneyAmount;

        public int MoneyAmount { get; private set; }

        public event Action OnMoneyAmountChanged;

        private void Awake()
        {
            Instance = this;
            MoneyAmount = startingMoneyAmount;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Buy(options[0]);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Buy(options[1]);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                Buy(options[2]);
            }
        }

        private void Buy(PlacingObjectSO placingObjectSO)
        {
            if (MoneyAmount >= placingObjectSO.price)
            {
                MoneyAmount -= placingObjectSO.price;
                OnMoneyAmountChanged?.Invoke();
            }
        }

        public void SpendMoney(int amount)
        {
            MoneyAmount -= amount;
            OnMoneyAmountChanged?.Invoke();
        }
    }
}