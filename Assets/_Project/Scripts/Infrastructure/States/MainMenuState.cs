using UnityEngine;

namespace _Project.Scripts.Infrastructure.States
{
    public class MainMenuState : IState
    {
        private ISceneLoader _sceneLoader;

        public MainMenuState(ISceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }

        public void Enter()
        {
            Debug.Log("Entered MainMenuState");

            _sceneLoader.Load(InfrastructureAssetPath.MAIN_MENU_SCENE);
        }

        public void Exit()
        {
        }
    }
}