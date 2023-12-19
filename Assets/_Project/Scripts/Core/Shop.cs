using System;

namespace _Project.Scripts.Core
{
    public class Shop : IShop
    {
        public int MoneyAmount { get; private set; }
        public event Action OnMoneyAmountChanged;

        public Shop(int moneyAmount)
        {
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
    }
    
    public interface IShop
    {
        int MoneyAmount { get; }
        event Action OnMoneyAmountChanged;
        void SpendMoney(int amount);
        void EarnMoney(int amount);
    }
}