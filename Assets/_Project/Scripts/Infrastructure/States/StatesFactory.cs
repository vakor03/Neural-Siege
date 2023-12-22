using Zenject;

namespace _Project.Scripts.Infrastructure.States
{
    public class StatesFactory
    {
        private DiContainer _container;

        public StatesFactory(DiContainer container)
        {
            _container = container;
        }

        public TState Create<TState>() where TState : IExitableState
        {
            return _container.Instantiate<TState>();
        }
    }
}