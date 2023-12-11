using UnityEngine;

namespace _Project.Scripts.Core
{
    public abstract class Weapon : MonoBehaviour
    {
        [SerializeField] private WeaponStrategy weaponStrategy;
        [SerializeField] private Transform firePoint;
        [SerializeField] private int layer;

        private void OnValidate()
        {
            layer = gameObject.layer;
            weaponStrategy.Initialize();
        }
    }
}