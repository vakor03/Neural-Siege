using _Project.Scripts.Core;
using _Project.Scripts.Core.Enemies;
using _Project.Scripts.Core.GridSystem;
using _Project.Scripts.Core.Towers;
using _Project.Scripts.Core.UI;
using _Project.Scripts.Infrastructure.States.Global;
using UnityEngine;

namespace _Project.Scripts.Infrastructure.States.Gameplay
{
    public class ResetGameplayAndGoToMainMenuState : IState
    {
        private PlacementSystem _placementSystem;
        private TowersController _towersController;
        private Shop _shop;
        private EnemiesAccessor _enemiesAccessor;
        private EnemySpawner _enemySpawner;
        private PlanningTimer _planningTimer;
        private GameOverUI _gameOverUI;
        private IPlayerBase _playerBase;
        private ScoreCounter _scoreCounter;
        private GameStateMachine _gameStateMachine;

        public ResetGameplayAndGoToMainMenuState(
            PlacementSystem placementSystem,
            TowersController towersController,
            Shop shop,
            EnemiesAccessor enemiesAccessor,
            EnemySpawner enemySpawner,
            PlanningTimer planningTimer,
            GameOverUI gameOverUI,
            IPlayerBase playerBase,
            ScoreCounter scoreCounter,
            GameStateMachine gameStateMachine)
        {
            _placementSystem = placementSystem;
            _towersController = towersController;
            _shop = shop;
            _enemiesAccessor = enemiesAccessor;
            _enemySpawner = enemySpawner;
            _planningTimer = planningTimer;
            _gameOverUI = gameOverUI;
            _playerBase = playerBase;
            _scoreCounter = scoreCounter;
            _gameStateMachine = gameStateMachine;
        }

        public void Exit()
        {
        }

        private void ResetSystems()
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
        }

        public void Enter()
        {
            Time.timeScale = 1f;

            ResetSystems();

            _gameStateMachine.Enter<MainMenuState>();
        }
    }
}