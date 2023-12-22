namespace _Project.Scripts.Infrastructure.States
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
            _sceneLoader.Load(InfrastructureAssetPath.GAME_SCENE);
        }
    }
}