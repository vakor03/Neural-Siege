using UnityEngine;

namespace _Project.Scripts.Core.Enemies
{
    [CreateAssetMenu(menuName = "Create EnemyStatsSO", fileName = "EnemyStatsSO", order = 0)]
    public class EnemyStatsSO : ScriptableObject
    {
        public EnemyType enemyType;
        public EnemyStats enemyStats;
    }
}