using _Project.Scripts.Core.Enemies;
using UnityEngine;

namespace _Project.Scripts.Algorithms.GA.Chromosomes
{
    public class ChromosomeFactory : IChromosomeFactory
    {
        private EnemyType[] _availableEnemyTypes = new EnemyType[]
        {
            EnemyType.Casual,
            EnemyType.Fast,
            EnemyType.Spawner,
            EnemyType.Tank
        };
        public Chromosome CreateRandom(int length)
        {
            EnemyType[] enemyWave = new EnemyType[length];
            for (int i = 0; i < length; i++)
            {
                enemyWave[i] = _availableEnemyTypes[Random.Range(0, _availableEnemyTypes.Length)];
            }
            return new Chromosome(enemyWave);
        }
    }
}