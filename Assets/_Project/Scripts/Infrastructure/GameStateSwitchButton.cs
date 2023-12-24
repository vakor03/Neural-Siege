using _Project.Scripts.Infrastructure.States;
using _Project.Scripts.Infrastructure.States.Gameplay;
using _Project.Scripts.Infrastructure.States.Global;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Project.Scripts.Infrastructure
{
    public class GameStateSwitchButton : MonoBehaviour
    {
        public enum TargetState
        {
            None = 0,
            Loading = 1,
            MainScene = 2,
            MainMenu = 3,
            Quit = 4,
            BacktrackingGameplay = 5,
            ManualGameplay = 6,
        }

        [SerializeField] private TargetState targetState = 0;
        [SerializeField] private Button button;

        private GameStateMachine _gameStateMachine;
        private PathCreationStateChoice _pathCreationStateChoice;

        [Inject]
        private void Construct(GameStateMachine gameStateMachine,PathCreationStateChoice pathCreationStateChoice)
        {
            _gameStateMachine = gameStateMachine;   
            _pathCreationStateChoice = pathCreationStateChoice;
        }

        private void OnEnable() =>
            button.onClick.AddListener(OnClick);

        private void OnDisable() =>
            button.onClick.RemoveListener(OnClick);

        private void OnClick()
        {
            switch (targetState)
            {
                case TargetState.Loading:
                    _gameStateMachine.Enter<GameLoadingState>();
                    break;
                // case TargetState.MainScene:
                //     _gameStateMachine.Enter<MainSceneState>();
                //     break;
                case TargetState.MainMenu:
                    _gameStateMachine.Enter<MainMenuState>();
                    break;
                case TargetState.Quit:
                    _gameStateMachine.Enter<QuitGameState>();
                    break;
                case TargetState.BacktrackingGameplay:
                    _pathCreationStateChoice.Type = PathCreationStateChoice.PathCreationType.Automatic;
                    _gameStateMachine.Enter<MainSceneState>();
                    break;
                case TargetState.ManualGameplay:
                    _pathCreationStateChoice.Type = PathCreationStateChoice.PathCreationType.Manual;
                    _gameStateMachine.Enter<MainSceneState>();
                    break;
                default:
                    Debug.LogError("Not valid option");
                    break;
            }
        }
    }
}