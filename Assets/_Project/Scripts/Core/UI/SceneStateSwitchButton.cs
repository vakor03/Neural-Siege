using _Project.Scripts.Infrastructure.States;
using _Project.Scripts.Infrastructure.States.Gameplay;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Project.Scripts.Core.UI
{
    public class SceneStateSwitchButton : MonoBehaviour
    {
        public enum TargetState
        {
            None = 0,
            AutomaticPathCreation = 1,
            ManualPathCreation = 2,
            Planning = 3,
            Wave = 4,
            GameOver = 5,
            MainMenu = 6,
            PrepareScene = 7,
        }

        [SerializeField] private TargetState targetState = 0;
        [SerializeField] private Button button;

        private SceneStateMachine _sceneStateMachine;

        [Inject]
        private void Construct(SceneStateMachine sceneStateMachine)
        {
            _sceneStateMachine = sceneStateMachine;
        }

        private void OnEnable() =>
            button.onClick.AddListener(OnClick);

        private void OnDisable() =>
            button.onClick.RemoveListener(OnClick);

        private void OnClick()
        {
            switch (targetState)
            {
                // case TargetState.AutomaticPathCreation:
                //     _sceneStateMachine.Enter<PathCreationState>();
                //     break;
                case TargetState.Wave:
                    _sceneStateMachine.Enter<WaveState>();
                    break;
                case TargetState.PrepareScene:
                    _sceneStateMachine.Enter<PrepareSceneState>();
                    break;
                case TargetState.MainMenu:
                    _sceneStateMachine.Enter<ResetGameplayAndGoToMainMenuState>();
                    break;
                default:
                    Debug.LogError("Not valid option");
                    break;
            }
        }
    }
}