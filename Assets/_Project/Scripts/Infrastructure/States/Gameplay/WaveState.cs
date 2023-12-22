using _Project.Scripts.Core;
using _Project.Scripts.Core.Enemies;

namespace _Project.Scripts.Infrastructure.States.Gameplay
{
    public class WaveState : IState
    {
        private GeneticAlgorithmWaveCreator _geneticAlgorithmWaveCreator;
        private EnemySpawner _enemySpawner;
        private SceneStateMachine _sceneStateMachine;
        private EnemiesAccessor _enemiesAccessor;
        private IPlayerBase _playerBase;

        public WaveState(
            GeneticAlgorithmWaveCreator geneticAlgorithmWaveCreator,
            EnemySpawner enemySpawner,
            SceneStateMachine sceneStateMachine,
            EnemiesAccessor enemiesAccessor,
            IPlayerBase playerBase)
        {
            _geneticAlgorithmWaveCreator = geneticAlgorithmWaveCreator;
            _enemySpawner = enemySpawner;
            _sceneStateMachine = sceneStateMachine;
            _enemiesAccessor = enemiesAccessor;
            _playerBase = playerBase;
        }
        public void Exit()
        {
            _enemiesAccessor.OnAllEnemiesDied -= OnWaveFinished;
            _playerBase.OnDeath -= OnPlayerBaseDeath;
        }

        public void Enter()
        {
            _playerBase.OnDeath += OnPlayerBaseDeath;
            _enemiesAccessor.OnAllEnemiesDied += OnWaveFinished;
            var wave = _geneticAlgorithmWaveCreator.CreateWave();
            _enemySpawner.SpawnWave(wave);
        }

        private void OnPlayerBaseDeath()
        {
            _sceneStateMachine.Enter<GameOverState>();
        }

        private void OnWaveFinished()
        {
            _sceneStateMachine.Enter<PlanningState>();
        }
    }
}