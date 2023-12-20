using UnityEngine;

namespace _Project.Scripts.Core.Enemies
{
    [CreateAssetMenu(menuName = "Create EnemyWave", fileName = "EnemyWave", order = 0)]
    public class EnemyWaveSO : ScriptableObject
    {
        public EnemyType[] enemies;
    }
}