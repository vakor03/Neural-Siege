using KBCore.Refs;
using UnityEngine;

namespace _Project.Scripts.Core
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Material healthBarMaterial;
        [SerializeField, Parent] private InterfaceRef<IHaveHealth> haveHealthRef;
        
        private Material _healthBarInstance;
        private static readonly int kFulfill = Shader.PropertyToID("_Fulfill");

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
            haveHealthRef.Value.OnHealthChanged += ChangeHealthbarFulfillness;
            ChangeHealthbarFulfillness();
        }

        private void ChangeHealthbarFulfillness()
        {
            var health = haveHealthRef.Value.CurrentHealth 
                         / haveHealthRef.Value.MaxHealth;
            _healthBarInstance.SetFloat(kFulfill, health);
        }
    }
}