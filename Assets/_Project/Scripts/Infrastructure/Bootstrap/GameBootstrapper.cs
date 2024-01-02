using _Project.Scripts.Infrastructure.States;
using _Project.Scripts.Infrastructure.States.Gameplay;
using _Project.Scripts.Infrastructure.States.Global;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Infrastructure.Bootstrap
{
    public class GameBootstrapper : MonoBehaviour
    {
        private GameStateMachine _gameStateMachine;
        private StatesFactory _statesFactory;

        [Inject]
        private void Construct(GameStateMachine gameStateMachine, StatesFactory statesFactory)
        {
            _gameStateMachine = gameStateMachine;
            _statesFactory = statesFactory;
        }

        private void Start()
        {
            _gameStateMachine.RegisterState(_statesFactory.Create<GameBootstrapState>());
            _gameStateMachine.RegisterState(_statesFactory.Create<MainMenuState>());
            _gameStateMachine.RegisterState(_statesFactory.Create<QuitGameState>());
            _gameStateMachine.RegisterState(_statesFactory.Create<MainSceneState>());

            _gameStateMachine.Enter<GameBootstrapState>();

            DontDestroyOnLoad(this);
        }
    }
}