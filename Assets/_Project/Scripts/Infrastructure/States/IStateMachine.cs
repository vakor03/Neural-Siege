namespace _Project.Scripts.Infrastructure.States
{
    public interface IStateMachine
    {
        void Enter<TState>() where TState : class, IState;
        void RegisterState<TState>(TState state) where TState : IExitableState;
    }
}