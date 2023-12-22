namespace _Project.Scripts.Infrastructure.States
{
    public class GameBootstrapState : IState
    {
        private GameStateMachine _gameStateMachine;

        public GameBootstrapState(GameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        }

        public void Enter()
        {
            InitServices();

            _gameStateMachine.Enter<MainMenuState>();
        }

        private void InitServices()
        {
            // noop
        }

        public void Exit()
        {
            // noop
        }
    }
}