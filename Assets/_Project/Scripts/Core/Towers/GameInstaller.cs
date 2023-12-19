using _Project.Scripts.Core.Enemies;
using _Project.Scripts.Core.GridSystem;
using UnityEngine.Serialization;
using Zenject;

namespace _Project.Scripts.Core.Towers
{
    public class GameInstaller : MonoInstaller
    {
        public EnemyFactory enemyFactoryPrefab;
        public int initialShopMoneyAmount;
        public int playerBaseHealth = 5;

        public PlacementSystem placementSystem;

        public override void InstallBindings()
        {
            BindEnemyFactory();
            BindShop();
            BindPlayerBase();
            BindPlacementSystem();
        }

        private void BindPlacementSystem()
        {
            Container.Bind<PlacementSystem>()
                .FromInstance(placementSystem).AsSingle();
        }

        private void BindPlayerBase()
        {
            Container.Bind<IPlayerBase>().To<PlayerBase>().AsSingle().WithArguments(playerBaseHealth);
        }

        private void BindShop()
        {
            Container.Bind<IShop>().To<Shop>().AsSingle().WithArguments(initialShopMoneyAmount);
        }

        private void BindEnemyFactory()
        {
            Container.Bind<IEnemyFactory>().FromComponentInNewPrefab(enemyFactoryPrefab).AsSingle();
        }
    }
}