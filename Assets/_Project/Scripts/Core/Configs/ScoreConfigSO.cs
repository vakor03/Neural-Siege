using _Project.Scripts.Core.Enemies;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace _Project.Scripts.Core.Configs
{
    [CreateAssetMenu(menuName = "Configs/ScoreConfig", fileName = "ScoreConfigSO", order = 0)]
    public class ScoreConfigSO : ScriptableObject
    {
        public SerializedDictionary<EnemyType, int> scorePerEnemy;
    }
}