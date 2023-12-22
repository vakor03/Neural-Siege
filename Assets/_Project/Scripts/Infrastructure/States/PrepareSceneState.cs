using _Project.Scripts.Core;
using _Project.Scripts.Core.Enemies;
using _Project.Scripts.Core.GridSystem;
using _Project.Scripts.Core.Towers;

namespace _Project.Scripts.Infrastructure.States
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

        public PrepareSceneState(SceneStateMachine sceneStateMachine,
            PlacementSystem placementSystem,
            TowersController towersController,
            Shop shop,
            EnemiesAccessor enemiesAccessor,
            EnemySpawner enemySpawner,
            PlanningTimer planningTimer,
            GameOverUI gameOverUI,
            IPlayerBase playerBase)
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
            
            _sceneStateMachine.Enter<AutomaticPathCreationState>();
        }
    }
}