using TMPro;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Core.UI
{
    public class PlanningTimerUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI timerText;

        private PlanningTimer _planningTimer;
        
        [Inject]
        private void Construct(PlanningTimer planningTimer)
        {
            _planningTimer = planningTimer;
        }
        
        private void Start()
        {
            _planningTimer.OnTimerTick += OnTimerTick;
            _planningTimer.OnTimerFinished += OnTimerFinished;
            OnTimerTick();
        }

        private void OnTimerFinished()
        {
            timerText.text = "--:--";
        }

        private void OnTimerTick()
        {
            int wholeSeconds = (int) _planningTimer.TimeLeft;
            int milliseconds = (int)((_planningTimer.TimeLeft - wholeSeconds)*100) ;
            timerText.text = $"{wholeSeconds:00}:{milliseconds:00}";
        }

        private void OnDestroy()
        {
            _planningTimer.OnTimerTick -= OnTimerTick;
            _planningTimer.OnTimerFinished -= OnTimerFinished;
        }
    }
}