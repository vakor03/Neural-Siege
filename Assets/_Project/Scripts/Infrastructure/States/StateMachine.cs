using System.Collections.Generic;

namespace _Project.Scripts.Infrastructure.States
{
    public abstract class StateMachine : IStateMachine
    {
        private readonly Dictionary<System.Type, IExitableState> _registeredStates = new();
        private IExitableState _currentState;

        public void Enter<TState>() where TState : class, IState
        {
            var newState = ChangeState<TState>();
            newState.Enter();
        }
        
        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>
        {
            var newState = ChangeState<TState>();
            newState.Enter(payload);
        }

        public void RegisterState<TState>(TState state) where TState : IExitableState
        {
            _registeredStates.Add(typeof(TState), state);
        }

        public TState ChangeState<TState>() where TState : class, IExitableState
        {
            if (_currentState != null)
                _currentState.Exit();

            TState state = GetState<TState>();
            _currentState = state;

            return state;
        }

        private TState GetState<TState>() where TState : class, IExitableState
        {
            return _registeredStates[typeof(TState)] as TState;
        }
    }
}