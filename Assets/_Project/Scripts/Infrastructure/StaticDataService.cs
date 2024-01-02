using System.Collections.Generic;
using _Project.Scripts.Core.Configs;
using _Project.Scripts.Core.Enemies;
using _Project.Scripts.Infrastructure.AssetProviders;
using Zenject;

namespace _Project.Scripts.Infrastructure
{
    public class StaticDataService : IInitializable
    {
        private IAssetProvider _assetProvider;
        private Dictionary<EnemyType, EnemyStatsSO> _enemyStats;
        private EnemyPathConfigSO _enemyPathConfig;
        private EnemyMoneyConfigSO _enemyMoneyConfig;
        private ScoreConfigSO _enemyScoreConfig;

        public StaticDataService(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
            _enemyStats = new Dictionary<EnemyType, EnemyStatsSO>();
        }
        
        public ScoreConfigSO GetEnemyScoreConfig()
        {
            return _enemyScoreConfig;
        }

        public EnemyStatsSO GetEnemyStats(EnemyType enemyType)
        {
            return _enemyStats[enemyType];
        }

        public EnemyPathConfigSO GetEnemyPathConfig()
        {
            return _enemyPathConfig;
        }

        public EnemyMoneyConfigSO GetEnemyMoneyConfig()
        {
            return _enemyMoneyConfig;
        }

        private void LoadEnemyPathConfig()
        {
            _enemyPathConfig = _assetProvider.Load<EnemyPathConfigSO>(AssetPath.ENEMY_PATH_CONFIG);
        }

        public void Initialize()
        {
            LoadEnemyStats();
            LoadEnemyPathConfig();
            LoadEnemyMoneyConfig();
            LoadEnemyScoreConfig();
        }

        private void LoadEnemyStats()
        {
            var enemyStats = _assetProvider.LoadAll<EnemyStatsSO>(AssetPath.ENEMY_STATS);
            foreach (var enemyStat in enemyStats)
            {
                _enemyStats.Add(enemyStat.enemyType, enemyStat);
            }
        }

        private void LoadEnemyMoneyConfig()
        {
            _enemyMoneyConfig = _assetProvider.Load<EnemyMoneyConfigSO>(AssetPath.ENEMY_MONEY_CONFIG);
        }

        private void LoadEnemyScoreConfig()
        {
            _enemyScoreConfig = _assetProvider.Load<ScoreConfigSO>(AssetPath.ENEMY_SCORE_CONFIG);
        }
    }
}