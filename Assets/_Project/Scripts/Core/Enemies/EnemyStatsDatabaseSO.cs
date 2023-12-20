using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace _Project.Scripts.Core.Enemies
{
    [CreateAssetMenu(menuName = "Create EnemyStatsDatabaseSO", fileName = "EnemyStatsDatabaseSO", order = 0)]
    public class EnemyStatsDatabaseSO : ScriptableObject
    {
        public SerializedDictionary<EnemyType, EnemyStatsSO> enemyStats;
    }
}