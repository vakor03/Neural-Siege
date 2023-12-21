using System.Collections.Generic;
using _Project.Scripts.Core.Towers;

namespace _Project.Scripts.Algorithms.GA.Structs
{
    public class TileStatsGA
    {
        public float Distance;
        public List<TowerStatsGA> Towers;

        public TileStatsGA(float distance, List<TowerStatsGA> towers)
        {
            Distance = distance;
            Towers = towers;
        }
    }
}