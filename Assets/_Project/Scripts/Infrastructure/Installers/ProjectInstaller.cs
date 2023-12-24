using _Project.Scripts.Core.Enemies;
using _Project.Scripts.Infrastructure.AssetProviders;
using _Project.Scripts.Infrastructure.SceneLoading;
using _Project.Scripts.Infrastructure.States;
using _Project.Scripts.Infrastructure.States.Gameplay;
using Zenject;

namespace _Project.Scripts.Infrastructure.Installers
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindPathCreationStateChoice();

            BindSceneLoader();

            BindAssetProvider();

            BindEnemyPathCreatorFactory();

            BindGameStateMachine();

            BindLoadingCurtain();
        }

        private void BindPathCreationStateChoice()
        {
            Container.Bind<PathCreationStateChoice>().AsSingle();
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