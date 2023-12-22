using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Project.Scripts.Infrastructure.States
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
                case TargetState.AutomaticPathCreation:
                    _sceneStateMachine.Enter<AutomaticPathCreationState>();
                    break;
                case TargetState.Wave:
                    _sceneStateMachine.Enter<WaveState>();
                    break;
                case TargetState.PrepareScene:
                    _sceneStateMachine.Enter<PrepareSceneState>();
                    break;
                default:
                    Debug.LogError("Not valid option");
                    break;
            }
        }
    }
}