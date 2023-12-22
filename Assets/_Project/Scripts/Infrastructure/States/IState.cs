namespace _Project.Scripts.Infrastructure.States
{
    public interface IState : IExitableState
    {
        void Enter();
    }
}