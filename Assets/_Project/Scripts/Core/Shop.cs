using System;
using _Project.Scripts.Core.Configs;
using _Project.Scripts.Core.Enemies;
using _Project.Scripts.Infrastructure.AssetProviders;

namespace _Project.Scripts.Core
{
    public class Shop
    {
        private EnemyMoneyConfigSO _enemyMoneyConfig;
        private int _initialMoneyAmount;
        private EnemiesAccessor _enemiesAccessor;
        public int MoneyAmount { get; private set; }
        public event Action OnMoneyAmountChanged;

        public Shop(int moneyAmount, EnemiesAccessor enemiesAccessor, IAssetProvider assetProvider)
        {
            _initialMoneyAmount = moneyAmount;
            MoneyAmount = moneyAmount;
            _enemiesAccessor = enemiesAccessor;
            _enemiesAccessor.OnEnemyDiedFromPlayer += EarnMoney;
            _enemyMoneyConfig = assetProvider.Load<EnemyMoneyConfigSO>(AssetPath.ENEMY_MONEY_CONFIG);
        }

        private void EarnMoney(Enemy enemy)
        {
            EarnMoney(_enemyMoneyConfig.moneyPerEnemy[enemy.EnemyType]);
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