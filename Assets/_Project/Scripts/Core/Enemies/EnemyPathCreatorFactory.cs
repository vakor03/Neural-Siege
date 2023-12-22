using _Project.Scripts.Infrastructure;
using _Project.Scripts.Infrastructure.AssetProviders;
using _Project.Scripts.Infrastructure.States;
using Zenject;

namespace _Project.Scripts.Core.Enemies
{
    public class EnemyPathCreatorFactory
    {
        private DiContainer _container;
        private IAssetProvider _assetProvider;

        public EnemyPathCreatorFactory(DiContainer container, IAssetProvider assetProvider)
        {
            _container = container;
            _assetProvider = assetProvider;
        }
        
        public BacktrackingPathCreation Create()
        {
            var enemyPathCreator = _container.Instantiate<BacktrackingPathCreation>();
            enemyPathCreator.Initialize(_assetProvider.Load<EnemyPathConfigSO>(InfrastructureAssetPath.ENEMY_PATH_CONFIG));
            return enemyPathCreator;
        }
    }
}