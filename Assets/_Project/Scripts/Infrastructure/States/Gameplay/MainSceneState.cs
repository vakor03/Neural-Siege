using _Project.Scripts.Infrastructure.AssetProviders;
using _Project.Scripts.Infrastructure.SceneLoading;

namespace _Project.Scripts.Infrastructure.States.Gameplay
{
    public class MainSceneState : IState
    {
        private ISceneLoader _sceneLoader;

        public MainSceneState(ISceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }

        public void Exit()
        {
        }

        public void Enter()
        {
            _sceneLoader.Load(AssetPath.GAME_SCENE);
        }
    }
}