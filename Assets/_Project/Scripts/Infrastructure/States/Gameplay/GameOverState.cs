using _Project.Scripts.Core.Enemies;
using _Project.Scripts.Core.UI;

namespace _Project.Scripts.Infrastructure.States.Gameplay
{
    public class GameOverState : IState
    {
        private GameOverUI _gameOverUI;
        private EnemiesAccessor _enemiesAccessor;

        public GameOverState(GameOverUI gameOverUI,
            EnemiesAccessor enemiesAccessor)
        {
            _gameOverUI = gameOverUI;
            _enemiesAccessor = enemiesAccessor;
        }

        public void Exit()
        {
            _gameOverUI.Hide();
        }

        public void Enter()
        {
            _enemiesAccessor.Clear();
            _gameOverUI.Show();
        }
    }
}