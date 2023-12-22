using UnityEngine;

namespace _Project.Scripts.Infrastructure.States
{
    public class GameOverUI : MonoBehaviour
    {
        [SerializeField] private GameObject panel;

        public void Hide()
        {
            panel.SetActive(false);
        }

        public void Show()
        {
            panel.SetActive(true);
        }
    }
}