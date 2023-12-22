using _Project.Scripts.Core.Enemies;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace _Project.Scripts.Core.Configs
{
    [CreateAssetMenu(menuName = "Create EnemyStatsDatabaseSO", fileName = "EnemyStatsDatabaseSO", order = 0)]
    public class EnemyStatsConfigSO : ScriptableObject
    {
        public SerializedDictionary<EnemyType, EnemyStatsSO> enemyStats;
    }
}