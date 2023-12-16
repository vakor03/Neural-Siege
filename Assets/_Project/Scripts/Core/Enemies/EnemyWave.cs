using UnityEngine;

namespace _Project.Scripts.Core.Enemies
{
    [CreateAssetMenu(menuName = "Create EnemyWave", fileName = "EnemyWave", order = 0)]
    public class EnemyWave : ScriptableObject
    {
        public Enemy[] enemies;
    }
}