using System;
using UnityEngine;

namespace _Project.Scripts
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