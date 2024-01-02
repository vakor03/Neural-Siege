using _Project.Scripts.Core.Enemies;
using _Project.Scripts.Core.PathCreation;
using _Project.Scripts.Core.Towers;
using _Project.Scripts.Core.WaypointSystem;
using UnityEngine;
using EnemyPathCreatorFactory = _Project.Scripts.Core.PathCreation.EnemyPathCreatorFactory;

namespace _Project.Scripts.Infrastructure.States.Gameplay
{
    public class PathCreationState : IState
    {
        private IPathCreationStrategy _pathCreationStrategy;
        private SceneStateMachine _sceneStateMachine;
        private WaypointsHolderFactory _waypointsHolderFactory;
        private EnemySpawner _enemySpawner;
        private readonly PathCreationStateChoice _pathCreationChoice;
        private readonly EnemyPathCreatorFactory _enemyPathCreatorFactory;
        private ValidatePathButtonUI _validatePathButtonUI;
        private StaticDataService _staticDataService;

        public PathCreationState(
            SceneStateMachine sceneStateMachine,
            WaypointsHolderFactory waypointsHolderFactory,
            EnemySpawner enemySpawner,
            PathCreationStateChoice pathCreationChoice,
            EnemyPathCreatorFactory enemyPathCreatorFactory,
            ValidatePathButtonUI validatePathButtonUI, 
            StaticDataService staticDataService)
        {
            _sceneStateMachine = sceneStateMachine;
            _waypointsHolderFactory = waypointsHolderFactory;
            _enemySpawner = enemySpawner;
            _pathCreationChoice = pathCreationChoice;
            _enemyPathCreatorFactory = enemyPathCreatorFactory;
            _validatePathButtonUI = validatePathButtonUI;
            _staticDataService = staticDataService;
        }

        private void OnPathCreated(Vector3[] path)
        {
            InitEnemySpawner(path);
            // _sceneStateMachine.Enter<PlanningState>();
        }

        public void Exit()
        {
            _pathCreationStrategy.OnPathCreated -= OnPathCreated;
            _validatePathButtonUI.OnClick -= OnValidatePathButtonClicked;
        }

        public void Enter()
        {
            switch (_pathCreationChoice.Type)
            {
                case PathCreationStateChoice.PathCreationType.Manual:
                    _pathCreationStrategy = _enemyPathCreatorFactory.Create<ManualPathCreation>();
                    break;
                case PathCreationStateChoice.PathCreationType.Automatic:
                    _pathCreationStrategy = _enemyPathCreatorFactory.Create<BacktrackingPathCreation>();
                    break;
            }

            _pathCreationStrategy.Initialize(_staticDataService.GetEnemyPathConfig());
            _validatePathButtonUI.gameObject.SetActive(true);
            _validatePathButtonUI.OnClick += OnValidatePathButtonClicked;
            _pathCreationStrategy.OnPathCreated += OnPathCreated;
            _pathCreationStrategy.StartCreatingPath();
        }

        private void OnValidatePathButtonClicked()
        {
            if (_pathCreationStrategy.IsPathValid())
            {
                _pathCreationStrategy.FinishCreatingPath();
                _validatePathButtonUI.gameObject.SetActive(false);
                _sceneStateMachine.Enter<PlanningState>();
            }
        }

        private void InitEnemySpawner(Vector3[] path)
        {
            Debug.Log("InitEnemySpawner");
            var waypointsHolder = _waypointsHolderFactory.Create(path);

            _enemySpawner.Initialize(waypointsHolder.Waypoints[0], waypointsHolder);
        }
    }
}