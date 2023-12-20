using System.Collections.Generic;
using _Project.Scripts.Core.Enemies;

namespace _Project.Scripts.Algorithms.GA
{
    public interface IWaveCreationAlgorithm
    {
        EnemyWave CreateEnemyWave(List<TileStats> tilesStats, int enemiesPerWave);
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