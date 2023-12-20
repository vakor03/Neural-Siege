using System.Collections.Generic;
using _Project.Scripts.Algorithms.GA.Chromosomes;
using _Project.Scripts.Core.Enemies;
using UnityEngine;

namespace _Project.Scripts.Algorithms.GA.Crossover
{
    public class SinglePointCrossover : ICrossoverLogic
    {
        private const float CROSSOVER_POINT = 0.5f;

        public List<Chromosome> Crossover(List<Chromosome> parents, int numberOfOffsprings)
        {
            List<Chromosome> offsprings = new List<Chromosome>();

            for (int i = 0; i < numberOfOffsprings / 2; i++)
            {
                var firstParent = parents[Random.Range(0, parents.Count)];
                var secondParent = parents[Random.Range(0, parents.Count)];

                var parentsOffsprings = Crossover(firstParent, secondParent);

                offsprings.Add(parentsOffsprings.Item1);
                offsprings.Add(parentsOffsprings.Item2);
            }

            return offsprings;
        }

        private (Chromosome, Chromosome) Crossover(Chromosome firstParent, Chromosome secondParent)
        {
            int length = firstParent.EnemyWave.Length;

            EnemyType[] enemyWave1 = new EnemyType[length];
            EnemyType[] enemyWave2 = new EnemyType[length];

            int i = 0;
            for (; i < length * CROSSOVER_POINT; i++)
            {
                enemyWave1[i] = firstParent.EnemyWave[i];
                enemyWave2[i] = secondParent.EnemyWave[i];
            }

            for (; i < length; i++)
            {
                enemyWave1[i] = secondParent.EnemyWave[i];
                enemyWave2[i] = firstParent.EnemyWave[i];
            }

            return (new Chromosome(enemyWave1), new Chromosome(enemyWave2));
        }
    }
}