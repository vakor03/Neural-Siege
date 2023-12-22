using TMPro;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Core.UI
{
    public class ScoreCounterUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI scoreText;

        private ScoreCounter _scoreCounter;

        [Inject]
        private void Construct(ScoreCounter scoreCounter)
        {
            _scoreCounter = scoreCounter;
        }

        private void Start()
        {
            _scoreCounter.OnScoreChanged += OnScoreChanged;
            OnScoreChanged();
        }

        private void OnScoreChanged()
        {
            scoreText.text = $"Score: {_scoreCounter.Score}";
        }

        private void OnDestroy()
        {
            _scoreCounter.OnScoreChanged -= OnScoreChanged;
        }
    }
}