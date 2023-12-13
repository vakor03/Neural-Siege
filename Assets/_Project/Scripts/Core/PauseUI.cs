using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Core
{
    public class PauseUI : MonoBehaviour
    {
        [SerializeField] private Transform parent;
        [SerializeField] private Button mainMenuButton;
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button restartButton;

        private void Awake()
        {
            mainMenuButton.onClick.AddListener(GoToMainMenu);
            resumeButton.onClick.AddListener(ResumeGame);
            restartButton.onClick.AddListener(Restart);
        }

        private void Restart()
        {
            GameManager.Instance.Restart();
        }

        private void Start()
        {
            GameManager.Instance.OnGamePaused += Show;
            GameManager.Instance.OnGameResumed += Hide;

            Hide();
        }

        private void ResumeGame()
        {
            GameManager.Instance.ResumeGame();
        }

        private void GoToMainMenu()
        {
            GameManager.Instance.GoToMainMenu();
        }

        private void OnDestroy()
        {
            mainMenuButton.onClick.RemoveListener(GoToMainMenu);
            resumeButton.onClick.RemoveListener(ResumeGame);
            restartButton.onClick.RemoveListener(Restart);

            GameManager.Instance.OnGamePaused -= Show;
            GameManager.Instance.OnGameResumed -= Hide;
        }

        private void Hide()
        {
            parent.gameObject.SetActive(false);
        }

        private void Show()
        {
            parent.gameObject.SetActive(true);
        }
    }
}