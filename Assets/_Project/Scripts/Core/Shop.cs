using System;

namespace _Project.Scripts.Core
{
    public class Shop
    {
        private int _initialMoneyAmount;
        public int MoneyAmount { get; private set; }
        public event Action OnMoneyAmountChanged;

        public Shop(int moneyAmount)
        {
            _initialMoneyAmount = moneyAmount;
            MoneyAmount = moneyAmount;
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

        public void Reset()
        {
            MoneyAmount = _initialMoneyAmount;
            OnMoneyAmountChanged?.Invoke();
        }
    }
}