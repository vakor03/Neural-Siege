using _Project.Scripts.Algorithms.GA.Chromosomes;
using _Project.Scripts.Core.Enemies;
using UnityEngine;

namespace _Project.Scripts.Algorithms.GA.Mutation
{
    public class ChromosomeMutator : IChromosomeMutator
    {
        private EnemyType[] _availableEnemyTypes = new[]
        {
            EnemyType.Casual,
            EnemyType.Fast,
            EnemyType.Spawner,
            EnemyType.Tank
        };
        
        public void Mutate(Chromosome chromosome, float mutationRate)
        {
            for (int i = 0; i < chromosome.EnemyWave.Length; i++)
            {
                if (Random.value < mutationRate)
                {
                    chromosome.EnemyWave[i] = _availableEnemyTypes[Random.Range(0, _availableEnemyTypes.Length)];
                }
            }
        }
    }
}