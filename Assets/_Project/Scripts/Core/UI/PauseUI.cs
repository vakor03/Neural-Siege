using _Project.Scripts.Core.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Core.UI
{
    public class PauseUI : MonoBehaviour
    {
        [SerializeField] private Transform parent;
        [SerializeField] private Button resumeButton;

        private void Awake()
        {
            parent.gameObject.SetActive(true);
            resumeButton.onClick.AddListener(ResumeGame);
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

        private void OnDestroy()
        {
            resumeButton.onClick.RemoveListener(ResumeGame);

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