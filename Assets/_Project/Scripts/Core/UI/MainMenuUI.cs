using Eflatun.SceneReference;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Core.UI
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] private Button startButton;
        [SerializeField] private Button quitButton;
        [SerializeField] private SceneReference nextScene;
        
        
        private void Start()
        {
            startButton.onClick.AddListener(StartGame);
            quitButton.onClick.AddListener(QuitGame);
        }

        private void StartGame()
        {
            SceneLoader.LoadScene(nextScene);
        }

        private void OnDestroy()
        {
            startButton.onClick.RemoveListener(StartGame);
            quitButton.onClick.RemoveListener(QuitGame);
        }

        private void QuitGame()
        {
            Utils.QuitGame();
        }
    }
}