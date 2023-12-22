using _Project.Scripts.Core;
using _Project.Scripts.Core.Enemies;
using _Project.Scripts.Core.GridSystem;
using _Project.Scripts.Core.PathCreation;
using _Project.Scripts.Core.Towers;
using _Project.Scripts.Core.UI;
using EnemyPathCreatorFactory = _Project.Scripts.Core.PathCreation.EnemyPathCreatorFactory;

namespace _Project.Scripts.Infrastructure.States.Gameplay
{
    public class PrepareSceneState : IState
    {
        private PlacementSystem _placementSystem;
        private TowersController _towersController;
        private Shop _shop;
        private EnemiesAccessor _enemiesAccessor;
        private EnemySpawner _enemySpawner;
        private PlanningTimer _planningTimer;
        private GameOverUI _gameOverUI;
        private IPlayerBase _playerBase;
        private SceneStateMachine _sceneStateMachine;
        private EnemyPathCreatorFactory _enemyPathCreatorFactory;
        private ScoreCounter _scoreCounter;


        public PrepareSceneState(SceneStateMachine sceneStateMachine,
            PlacementSystem placementSystem,
            TowersController towersController,
            Shop shop,
            EnemiesAccessor enemiesAccessor,
            EnemySpawner enemySpawner,
            PlanningTimer planningTimer,
            GameOverUI gameOverUI,
            IPlayerBase playerBase,
            EnemyPathCreatorFactory enemyPathCreatorFactory,
            ScoreCounter scoreCounter)
        {
            _sceneStateMachine = sceneStateMachine;
            _placementSystem = placementSystem;
            _towersController = towersController;
            _shop = shop;
            _enemiesAccessor = enemiesAccessor;
            _enemySpawner = enemySpawner;
            _planningTimer = planningTimer;
            _gameOverUI = gameOverUI;
            _playerBase = playerBase;
            _enemyPathCreatorFactory = enemyPathCreatorFactory;
            _scoreCounter = scoreCounter;
        }

        public void Exit()
        {
        }

        public void Enter()
        {
            _towersController.Clear();
            _placementSystem.Clear();
            _shop.Reset();
            _enemySpawner.Reset();
            _enemiesAccessor.Clear();
            _planningTimer.Reset();
            _gameOverUI.Hide();
            _playerBase.Reset();
            _scoreCounter.Reset();

            var enemyPathCreator = ChosenAutomatic ? 
                _enemyPathCreatorFactory.Create<BacktrackingPathCreation>() : 
                _enemyPathCreatorFactory.Create<ManualPathCreation>();

            
            _sceneStateMachine.Enter<PathCreationState, IPathCreationStrategy>(enemyPathCreator); // TODO: add ready button
        }

        public bool ChosenAutomatic { get; set; } = true;//TODO: move to another component or smth
    }
}