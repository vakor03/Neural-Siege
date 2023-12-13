using System;
using Eflatun.SceneReference;
using UnityEngine;

namespace _Project.Scripts.Core
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [SerializeField] private SceneReference mainMenuScene;
        [SerializeField] private SceneReference currentScene;
        
        private bool _isPaused;

        public Action OnGamePaused;
        public Action OnGameResumed;

        private void Awake()
        {
            Instance = this;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && !_isPaused)
            {
                PauseGame();
            }else if (Input.GetKeyDown(KeyCode.Escape) && _isPaused)
            {
                ResumeGame();
            }
        }

        public void PauseGame()
        {
            _isPaused = true;
            Time.timeScale = 0;
            OnGamePaused?.Invoke();
        }

        public void ResumeGame()
        {
            _isPaused = false;
            Time.timeScale = 1;
            OnGameResumed?.Invoke();
        }

        public void GoToMainMenu()
        {
            _isPaused = false;
            Time.timeScale = 1;
            SceneLoader.LoadScene(mainMenuScene);
        }

        public void Restart()
        {
            _isPaused = false;
            Time.timeScale = 1;
            SceneLoader.LoadScene(currentScene);
        }
    }
}