using _Project.Scripts.Core.Configs;
using _Project.Scripts.Core.PathCreation;
using _Project.Scripts.Infrastructure.AssetProviders;
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
            enemyPathCreator.Initialize(_assetProvider.Load<EnemyPathConfigSO>(AssetPath.ENEMY_PATH_CONFIG));
            return enemyPathCreator;
        }
    }
}