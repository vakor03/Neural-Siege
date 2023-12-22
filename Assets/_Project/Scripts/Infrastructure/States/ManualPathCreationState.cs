using _Project.Scripts.Core.Enemies;
using _Project.Scripts.Core.GridSystem;
using _Project.Scripts.Infrastructure.AssetProviders;
using UnityEngine;

namespace _Project.Scripts.Infrastructure.States
{
    public class ManualPathCreationState : IState
    {
        private ManualPathCreation _manualPathCreation;
        private SceneStateMachine _sceneStateMachine;
        private WaypointsHolderFactory _waypointsHolderFactory;
        private EnemySpawner _enemySpawner;

        public ManualPathCreationState(
            ManualPathCreation manualPathCreation,
            IAssetProvider assetProvider,
            SceneStateMachine sceneStateMachine,
            WaypointsHolderFactory waypointsHolderFactory, 
            EnemySpawner enemySpawner)
        {
            _manualPathCreation = manualPathCreation;
            _manualPathCreation.Initialize(
                assetProvider.Load<EnemyPathConfigSO>(InfrastructureAssetPath.ENEMY_PATH_CONFIG));
            _sceneStateMachine = sceneStateMachine;
            _waypointsHolderFactory = waypointsHolderFactory;
            _enemySpawner = enemySpawner;
        }

        public void Enter()
        {
            _manualPathCreation.OnPathCreated += OnPathCreated;
            _manualPathCreation.StartCreatingPath();
        }

        private void OnPathCreated(Vector3[] path)
        {
            InitEnemySpawner(path);
            _sceneStateMachine.Enter<PlanningState>();
        }

        public void Exit()
        {
            _manualPathCreation.OnPathCreated -= OnPathCreated;
        }
        
        private void InitEnemySpawner(Vector3[] path)
        {
            var waypointsHolder = _waypointsHolderFactory.Create(path);
        
            _enemySpawner.Initialize(waypointsHolder.Waypoints[0], waypointsHolder);
        }
    }
}