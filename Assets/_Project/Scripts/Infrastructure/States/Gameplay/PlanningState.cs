using _Project.Scripts.Core;

namespace _Project.Scripts.Infrastructure.States.Gameplay
{
    public class PlanningState : IState
    {
        private SceneStateMachine _sceneStateMachine;
        private PlanningTimer _planningTimer;

        public PlanningState(SceneStateMachine sceneStateMachine,
            PlanningTimer planningTimer)
        {
            _sceneStateMachine = sceneStateMachine;
            _planningTimer = planningTimer;
        }

        public void Exit()
        {
            _planningTimer.OnTimerFinished -= OnTimerFinished;
            _planningTimer.Reset();
        }

        public void Enter()
        {
            _planningTimer.OnTimerFinished += OnTimerFinished;
            _planningTimer.Start();
        }

        private void OnTimerFinished()
        {
            _sceneStateMachine.Enter<WaveState>();
        }
    }
}