using _Project.Scripts.Core.Enemies;
using KBCore.Refs;
using UnityEngine;

namespace _Project.Scripts.Core
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Material healthBarMaterial;
        [SerializeField] private EnemyHealth haveHealthRef;
        
        private Material _healthBarInstance;
        private static readonly int kFulfill = Shader.PropertyToID("_Fulfill");
        
        private void Awake()
        {
            _healthBarInstance = new Material(healthBarMaterial);
            GetComponent<Renderer>().material = _healthBarInstance;
        }

        private void Start()
        {
            haveHealthRef.OnHealthChanged += ChangeHealthbarFulfillness;
            ChangeHealthbarFulfillness();
        }

        private void ChangeHealthbarFulfillness()
        {
            var health = haveHealthRef.CurrentHealth 
                         / haveHealthRef.MaxHealth;
            _healthBarInstance.SetFloat(kFulfill, health);
        }
    }
}