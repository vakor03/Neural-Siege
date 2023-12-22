using _Project.Scripts.Core.Enemies;
using _Project.Scripts.Infrastructure.AssetProviders;

namespace _Project.Scripts.Infrastructure.States
{
    public class AutomaticPathCreationState : IState
    {
        private EnemyPathCreator _enemyPathCreator;
        private readonly SceneStateMachine _sceneStateMachine;

        public AutomaticPathCreationState(
            EnemyPathCreator enemyPathCreator,
            IAssetProvider assetProvider,
            SceneStateMachine sceneStateMachine)
        {
            _enemyPathCreator = enemyPathCreator;
            _enemyPathCreator.Initialize(
                assetProvider.Load<EnemyPathConfigSO>(InfrastructureAssetPath.ENEMY_PATH_CONFIG));
            _sceneStateMachine = sceneStateMachine;
        }

        public void Enter()
        {
            _enemyPathCreator.OnPathCreated += OnPathCreated;
            _enemyPathCreator.CreatePath();
        }

        private void OnPathCreated()
        {
            _sceneStateMachine.Enter<PlanningState>();
        }

        public void Exit()
        {
            _enemyPathCreator.OnPathCreated -= OnPathCreated;
        }
    }
}