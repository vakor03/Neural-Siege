using TMPro;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Core.UI
{
    public class PlayerBaseHealthUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text healthText;
        // [SerializeField] private Image healthBar;
        private IPlayerBase _playerBase;

        [Inject]
        private void Construct(IPlayerBase playerBase)
        {
            _playerBase = playerBase;
        }
        
        private void Start()
        {
            _playerBase.OnHealthChanged += UpdateHealth;
            UpdateHealth();
        }
        
        private void OnDestroy()
        {
            _playerBase.OnHealthChanged -= UpdateHealth;
        }

        private void UpdateHealth()
        {
            healthText.text = $"Health: {_playerBase.CurrentHealth}/{_playerBase.MaxHealth}";
            // healthBar.fillAmount = (float)_playerBase.CurrentHealth / _playerBase.MaxHealth;
        }
    }
}