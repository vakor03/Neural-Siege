using UnityEngine;

namespace _Project.Scripts.Core.Towers
{
    [CreateAssetMenu(menuName = "Create SimpleTowerStatsSO", fileName = "SimpleTowerStatsSO", order = 0)]
    public class SimpleTowerStatsSO : ScriptableObject
    {
        public float range;
        public float attackInterval;
        public Projectile projectilePrefab;
    }
}