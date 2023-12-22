using System;
using _Project.Scripts.Core.Configs;
using _Project.Scripts.Core.Enemies;
using _Project.Scripts.Infrastructure.AssetProviders;

namespace _Project.Scripts.Core
{
    public class ScoreCounter
    {
        private ScoreConfigSO _scoreConfig;
        private EnemiesAccessor _enemiesAccessor;

        public int Score { get; private set; }

        public event Action OnScoreChanged;

        public ScoreCounter(EnemiesAccessor enemiesAccessor, IAssetProvider assetProvider)
        {
            _scoreConfig = assetProvider.Load<ScoreConfigSO>(AssetPath.ENEMY_SCORE_CONFIG);
            _enemiesAccessor = enemiesAccessor;

            _enemiesAccessor.OnEnemyDiedFromPlayer += OnEnemyDied;
        }

        private void OnEnemyDied(Enemy obj)
        {
            Score += _scoreConfig.scorePerEnemy[obj.EnemyType];
            OnScoreChanged?.Invoke();
        }

        public void Reset()
        {
            Score = 0;
            OnScoreChanged?.Invoke();
        }
    }
}