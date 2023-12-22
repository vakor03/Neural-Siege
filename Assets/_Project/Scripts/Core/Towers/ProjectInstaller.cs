using _Project.Scripts.Core.Enemies;
using _Project.Scripts.Infrastructure;
using _Project.Scripts.Infrastructure.AssetProviders;
using _Project.Scripts.Infrastructure.States;
using Zenject;

namespace _Project.Scripts.Core.Towers
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindSceneLoader();

            BindAssetProvider();

            BindEnemyPathCreatorFactory();

            BindGameStateMachine();

            BindLoadingCurtain();
        }

        private void BindEnemyPathCreatorFactory()
        {
            Container.Bind<EnemyPathCreatorFactory>().AsSingle();
        }

        private void BindAssetProvider()
        {
            Container.BindInterfacesAndSelfTo<AssetProvider>().AsSingle();
        }

        private void BindLoadingCurtain()
        {
            Container.BindInterfacesAndSelfTo<LoadingCurtain>().AsSingle();
        }

        private void BindGameStateMachine()
        {
            Container.Bind<StatesFactory>().AsSingle();

            Container.Bind<GameStateMachine>().AsSingle();
        }

        private void BindSceneLoader()
        {
            Container.BindInterfacesAndSelfTo<SceneLoader>().AsSingle();
        }
    }
}