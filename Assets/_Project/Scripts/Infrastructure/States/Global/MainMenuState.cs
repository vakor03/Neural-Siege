using _Project.Scripts.Infrastructure.AssetProviders;
using _Project.Scripts.Infrastructure.SceneLoading;
using UnityEngine;

namespace _Project.Scripts.Infrastructure.States.Global
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

            _sceneLoader.Load(AssetPath.MAIN_MENU_SCENE);
        }

        public void Exit()
        {
        }
    }
}