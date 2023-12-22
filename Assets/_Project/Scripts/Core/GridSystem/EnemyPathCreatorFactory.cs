using _Project.Scripts.Core.Enemies;
using Zenject;

namespace _Project.Scripts.Core.GridSystem
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