using _Project.Scripts.Infrastructure.SceneLoading;
using UnityEngine;

namespace _Project.Scripts.Infrastructure.States.Global
{
    public class GameLoadingState : IState
    {
        private ILoadingCurtain _loadingCurtain;
        private ISceneLoader _sceneLoader;

        public GameLoadingState(ILoadingCurtain loadingCurtain, ISceneLoader sceneLoader)
        {
            _loadingCurtain = loadingCurtain;
            _sceneLoader = sceneLoader;
        }

        public void Enter()
        {
            Debug.Log($"Entered {nameof(GameLoadingState)}");
            
            _loadingCurtain.Show();

            _sceneLoader.Load("Game");

            _loadingCurtain.Hide();
        }

        public void Exit()
        {
            _loadingCurtain.Show();
        }
    }
}