using KBCore.Refs;
using UnityEngine;

namespace _Project.Scripts
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Material healthBarMaterial;
        [SerializeField, Parent] private Enemy enemy;

        private Material _healthBarInstance;

        private void OnValidate()
        {
            this.ValidateRefs();
        }
        private void Awake()
        {
            _healthBarInstance = new Material(healthBarMaterial);
            GetComponent<Renderer>().material = _healthBarInstance;
        }

        private void Start()
        {
            enemy.OnHealthChanged += ChangeHealthbarFulfillness;
            ChangeHealthbarFulfillness();
        }

        private void ChangeHealthbarFulfillness()
        {
            var health = enemy.CurrentHealth / enemy.MaxHealth;
            _healthBarInstance.SetFloat("_Fulfill", health);
        }
    }
}