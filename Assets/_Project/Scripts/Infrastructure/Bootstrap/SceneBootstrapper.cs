using _Project.Scripts.Infrastructure.States;
using _Project.Scripts.Infrastructure.States.Gameplay;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Infrastructure.Bootstrap
{
    public class SceneBootstrapper : MonoBehaviour
    {
        private SceneStateMachine _sceneStateMachine;
        private StatesFactory _statesFactory;

        [Inject]
        private void Construct(SceneStateMachine sceneStateMachine, StatesFactory statesFactory)
        {
            _sceneStateMachine = sceneStateMachine;
            _statesFactory = statesFactory;
        }

        private void Start()
        {
            _sceneStateMachine.RegisterState(_statesFactory.Create<PathCreationState>());
            // _sceneStateMachine.RegisterState(_statesFactory.Create<ManualPathCreationState>());
            _sceneStateMachine.RegisterState(_statesFactory.Create<WaveState>());
            _sceneStateMachine.RegisterState(_statesFactory.Create<PlanningState>());
            _sceneStateMachine.RegisterState(_statesFactory.Create<GameOverState>());
            _sceneStateMachine.RegisterState(_statesFactory.Create<PrepareSceneState>());
            _sceneStateMachine.RegisterState(_statesFactory.Create<ResetGameplayAndGoToMainMenuState>());
            
            _sceneStateMachine.Enter<PrepareSceneState>();
        }
    }
}