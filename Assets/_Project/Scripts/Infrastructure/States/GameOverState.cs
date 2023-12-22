using UnityEngine;

namespace _Project.Scripts.Infrastructure.States
{
    public class GameOverState : IState
    {
        private GameOverUI _gameOverUI;

        public GameOverState(GameOverUI gameOverUI)
        {
            _gameOverUI = gameOverUI;
        }

        public void Exit()
        {
            _gameOverUI.Hide();
        }

        public void Enter()
        {
            _gameOverUI.Show();
            Debug.Log("Game Over");
        }
    }
}