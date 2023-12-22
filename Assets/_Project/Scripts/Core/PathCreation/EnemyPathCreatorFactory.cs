using Zenject;

namespace _Project.Scripts.Core.PathCreation
{
    public class EnemyPathCreatorFactory
    {
        private DiContainer _container;
        
        public EnemyPathCreatorFactory(DiContainer container)
        {
            _container = container;
        }
        
        public IPathCreationStrategy Create<T>()where T : IPathCreationStrategy
        {
            return _container.Instantiate<T>();
        }
    }
}