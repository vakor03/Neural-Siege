using UnityEngine;

namespace _Project.Scripts.Core.GridSystem
{
    [CreateAssetMenu(menuName = "Create EnemyWave", fileName = "EnemyWave", order = 0)]
    public class EnemyWave : ScriptableObject
    {
        public Enemy[] enemies;
    }
}