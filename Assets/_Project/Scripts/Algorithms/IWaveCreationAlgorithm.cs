using System.Collections.Generic;
using _Project.Scripts.Algorithms.GA.Structs;
using _Project.Scripts.Core.Enemies;

namespace _Project.Scripts.Algorithms
{
    public interface IWaveCreationAlgorithm
    {
        EnemyWave CreateEnemyWave(List<TileStatsGA> tilesStats, int enemiesPerWave);
    }

    public class EnemyWave
    {
        public EnemyType[] enemies;

        public EnemyWave(EnemyType[] enemies)
        {
            this.enemies = enemies;
        }
    }
}