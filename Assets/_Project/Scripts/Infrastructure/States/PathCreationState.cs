using _Project.Scripts.Core.Enemies;
using _Project.Scripts.Core.GridSystem;
using _Project.Scripts.Infrastructure.AssetProviders;
using UnityEngine;

namespace _Project.Scripts.Infrastructure.States
{

    public class PathCreationState : IPayloadedState<IPathCreationStrategy>
    {
        private IPathCreationStrategy _pathCreationStrategy;
        private SceneStateMachine _sceneStateMachine;
        private WaypointsHolderFactory _waypointsHolderFactory;
        private EnemySpawner _enemySpawner;
        private readonly IAssetProvider _assetProvider;

        public PathCreationState(
            IAssetProvider assetProvider,
            SceneStateMachine sceneStateMachine,
            WaypointsHolderFactory waypointsHolderFactory,
            EnemySpawner enemySpawner)
        {
            _sceneStateMachine = sceneStateMachine;
            _waypointsHolderFactory = waypointsHolderFactory;
            _enemySpawner = enemySpawner;
            _assetProvider = assetProvider;
        }

        private void OnPathCreated(Vector3[] path)
        {
            InitEnemySpawner(path);
            _sceneStateMachine.Enter<PlanningState>();
        }

        public void Enter(IPathCreationStrategy payload)
        {
            _pathCreationStrategy = payload;
            _pathCreationStrategy.Initialize(
                _assetProvider.Load<EnemyPathConfigSO>(InfrastructureAssetPath.ENEMY_PATH_CONFIG));
            
            _pathCreationStrategy.OnPathCreated += OnPathCreated;
            _pathCreationStrategy.StartCreatingPath();
        }

        public void Exit()
        {
            _pathCreationStrategy.OnPathCreated -= OnPathCreated;
        }

        private void InitEnemySpawner(Vector3[] path)
        {
            var waypointsHolder = _waypointsHolderFactory.Create(path);

            _enemySpawner.Initialize(waypointsHolder.Waypoints[0], waypointsHolder);
        }
    }
}