using TMPro;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Core.UI
{
    public class GameOverUI : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private TextMeshProUGUI scoreText;
        
        private ScoreCounter _scoreCounter;

        [Inject]
        private void Construct(ScoreCounter scoreCounter)
        {
            _scoreCounter = scoreCounter;
        }

        public void Hide()
        {
            panel.SetActive(false);
        }

        public void Show()
        {
            scoreText.text = $"Score: {_scoreCounter.Score}";
            panel.SetActive(true);
        }
    }
}