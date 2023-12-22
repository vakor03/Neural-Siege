using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Algorithms;
using _Project.Scripts.Algorithms.GA;
using _Project.Scripts.Algorithms.GA.Structs;
using _Project.Scripts.Core.Towers;
using _Project.Scripts.Core.WaypointSystem;
using _Project.Scripts.Extensions;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Core
{
    public class GeneticAlgorithmWaveCreator
    {
        private int enemiesPerWave = 10;
        private IWaveCreationAlgorithm _algorithm;
        private TowersController _towersController;

        [Inject]
        private void Construct(GeneticAlgorithmFactory geneticAlgorithmFactory,
            TowersController towersController)
        {
            _towersController = towersController;
            _algorithm = geneticAlgorithmFactory.Create();
        }
    
        public EnemyWave CreateWave()
        {
            List<(Circle, TowerStatsGA)> towersInfo = _towersController.Towers
                .Select(tower => (new Circle(tower.transform.position, tower.Range), tower.GetTowerStatsGA()))
                .ToList();
        
            var tileStats = CalculateTileStats(towersInfo);
            EnemyWave wave = _algorithm.CreateEnemyWave(tileStats, enemiesPerWave);
            return wave;
        }

        private List<TileStatsGA> CalculateTileStats(List<(Circle, TowerStatsGA)> towersInfo)
        {
            WaypointsHolder waypointsHolder = WaypointsHolderFactory.Last;
            var waypoints = waypointsHolder.Waypoints;
            Rect[] rects = ConvertToRects(waypoints);
            float distance = 1;
            var tileStatsGA = rects.Select(r => new TileStatsGA(distance, GetTowerStatsInRects(r, towersInfo))).ToList();

            return tileStatsGA;
        }

        private Rect[] ConvertToRects(Vector3[] waypoints)
        {
            return waypoints.Select(waypoint => new Rect(waypoint.x - 0.5f, waypoint.y - 0.5f, 1, 1)).ToArray();
        }

        private List<TowerStatsGA> GetTowerStatsInRects(Rect rect, List<(Circle, TowerStatsGA)> towersInfo)
        {
            var towersInRect = towersInfo
                .Where(radius => radius.Item1.Intersects(rect))
                .Select(tower=>tower.Item2).ToList();
        
            return towersInRect;
        }
    }
}