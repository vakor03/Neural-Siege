using _Project.Scripts.Core.Enemies;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace _Project.Scripts.Core.Configs
{
    [CreateAssetMenu(menuName = "Configs/EnemyMoneyConfig", fileName = "EnemyMoneyConfigSO", order = 0)]
    public class EnemyMoneyConfigSO : ScriptableObject
    {
        public SerializedDictionary<EnemyType, int> moneyPerEnemy;
    }
}