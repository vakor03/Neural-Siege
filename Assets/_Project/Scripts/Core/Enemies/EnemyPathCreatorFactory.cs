using _Project.Scripts.Core.PathCreation;
using _Project.Scripts.Infrastructure;
using Zenject;

namespace _Project.Scripts.Core.Enemies
{
    public class EnemyPathCreatorFactory
    {
        private DiContainer _container;
        private StaticDataService _staticDataService;

        public EnemyPathCreatorFactory(DiContainer container, StaticDataService staticDataService)
        {
            _container = container;
            _staticDataService = staticDataService;
        }

        public BacktrackingPathCreation Create()
        {
            var enemyPathCreator = _container.Instantiate<BacktrackingPathCreation>();
            enemyPathCreator.Initialize(_staticDataService.GetEnemyPathConfig());
            return enemyPathCreator;
        }
    }
}