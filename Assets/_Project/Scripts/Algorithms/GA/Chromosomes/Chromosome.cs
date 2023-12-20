using _Project.Scripts.Core.Enemies;

namespace _Project.Scripts.Algorithms.GA.Chromosomes
{
    public class Chromosome
    {
        public EnemyType[] EnemyWave;

        public Chromosome(EnemyType[] enemyWave)
        {
            EnemyWave = enemyWave;
        }
    }
}