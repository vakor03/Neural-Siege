using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Algorithms.GA.Chromosomes;
using _Project.Scripts.Algorithms.GA.Structs;
using _Project.Scripts.Core.Enemies;
using _Project.Scripts.Core.WaypointSystem;
using _Project.Scripts.Extensions;
using UnityEngine;

namespace _Project.Scripts.Algorithms.GA.Evaluation
{
    public class ModelFitnessEvaluator : IModelFitnessEvaluator
    {
        private Dictionary<EnemyType, EnemyStatsGA> _enemyStats;

        // private Dictionary<TowerTypeSO, TowerStatsGA> _towerStats;
        public List<TileStatsGA> TilesStats { get; set; } = new();

        private readonly HashSet<TmpEnemyStats>
            _deadEnemiesStats = new(); // This can work not as expected cause of struct

        private readonly EnemySpawner _enemySpawner;

        private const float PER_ENEMY_REACHED_COEFFICIENT = 1000f;
        private const float WAYPOINT_REACHED_COEFFICIENT = 0 /*100f*/;
        private const float PRICE_COEFFICIENT = -1f;
        private const float DIVERSITY_COEFFICIENT = 100f;

        public ModelFitnessEvaluator(Dictionary<EnemyType, EnemyStatsGA> enemyStats, EnemySpawner enemySpawner)
        {
            _enemyStats = enemyStats;
            _enemySpawner = enemySpawner;
        }

        public float Evaluate(Chromosome chromosome)
        {
            float survivalScore = CalculateSurvivalScore(chromosome);
            float diversityScore = CalculateDiversityScore(chromosome);
            float priceScore = CalculatePriceScore(chromosome);

            float fitness = survivalScore + diversityScore + priceScore;

            return fitness;
        }

        private float CalculateDiversityScore(Chromosome chromosome)
        {
            var diversity = chromosome.EnemyWave.Distinct().Count();
            return Mathf.Pow(2, diversity) * DIVERSITY_COEFFICIENT;
        }

        private float CalculatePriceScore(Chromosome chromosome)
        {
            return chromosome.EnemyWave.Sum(e => _enemyStats[e].Price) * PRICE_COEFFICIENT;
        }

        private float CalculateSurvivalScore(Chromosome chromosome)
        {
            TilesStats.ForEach(ts => ts.Towers.ForEach(t => t.TimeShooting = 0));
            var survived = ConvertEnemyTypesToTempStats(chromosome.EnemyWave);
            float spawnRate = _enemySpawner.SpawnRate;

            for (var i = 0; i < survived.Count; i++)
            {
                var enemyTempStats = survived[i];
                enemyTempStats.TimeToCome = i * 1 / spawnRate;
                survived[i] = enemyTempStats;
            }

            int lastReachedWaypoint = 0;

            for (var i = 0; i < TilesStats.Count; i++)
            {
                if (survived.Count == 0) break;

                var tilesStat = TilesStats[i];
                survived = CalculateWhoWillSurviveOnTile(survived, tilesStat);
                lastReachedWaypoint = i;
            }

            float value;
            if (lastReachedWaypoint == TilesStats.Count - 1)
            {
                value = survived.Count * PER_ENEMY_REACHED_COEFFICIENT +
                        lastReachedWaypoint * WAYPOINT_REACHED_COEFFICIENT;
            }
            else
            {
                value = lastReachedWaypoint * WAYPOINT_REACHED_COEFFICIENT;
            }

            return value;
        }

        private List<TmpEnemyStats> CalculateWhoWillSurviveOnTile(List<TmpEnemyStats> tempEnemyStats,
            TileStatsGA tileStatsGA)
        {
            UpdateTimeToNextWaypoint(tempEnemyStats, tileStatsGA.Distance);
            var activeTowers = tileStatsGA.Towers;

            ApplySlowEffects(activeTowers, tempEnemyStats, tileStatsGA.Distance);

            AddNewlySpawned(tempEnemyStats, tileStatsGA.Distance, activeTowers);

            ApplyAoeDamage(activeTowers, tempEnemyStats);

            ApplySingleTargetDamage(activeTowers, tempEnemyStats);

            for (var i = 0; i < tempEnemyStats.Count; i++)
            {
                var enemyTempStats = tempEnemyStats[i];
                enemyTempStats.TimeToCome += enemyTempStats.TimeToNextWaypoint;
                tempEnemyStats[i] = enemyTempStats;
            }

            return tempEnemyStats.OrderBy(tmp => tmp.TimeToCome).ToList();
        }
        
        private void AddNewlySpawned(List<TmpEnemyStats> tempEnemyStats, float distance,
            List<TowerStatsGA> activeTowers)
        {
            const int maxNewlySpawned = 5; // no need to account all spawned
            List<TmpEnemyStats> newlySpawned = new();
            foreach (var enemyStats in tempEnemyStats)
            {
                int alreadySpawned = (int)(enemyStats.TimeToCome * enemyStats.ReproductionRate);
                int maxCanSpawn = (int)((enemyStats.TimeToNextWaypoint + enemyStats.TimeToCome) *
                                        enemyStats.ReproductionRate);

                int timeSpawned = maxCanSpawn - alreadySpawned;
                timeSpawned = Mathf.Clamp(timeSpawned, 0, maxNewlySpawned);
                for (int i = 0; i < timeSpawned; i++)
                {
                    var newlySpawnedEnemy = ConvertEnemyTypeToTempStats(enemyStats.SpawnedType);
                    newlySpawnedEnemy.TimeToCome = enemyStats.TimeToCome;
                    newlySpawned.Add(newlySpawnedEnemy);
                }
            }

            UpdateTimeToNextWaypoint(newlySpawned, distance);
            ApplySlowEffects(activeTowers, newlySpawned, distance);
            tempEnemyStats.AddRange(newlySpawned);
        }

        private void UpdateTimeToNextWaypoint(List<TmpEnemyStats> tempEnemyStats, float distanceBetweenWaypoints)
        {
            for (var i = 0; i < tempEnemyStats.Count; i++)
            {
                var enemyTempStats = tempEnemyStats[i];
                enemyTempStats.TimeToNextWaypoint = distanceBetweenWaypoints / enemyTempStats.Speed;
                tempEnemyStats[i] = enemyTempStats;
            }
        }

        private void ApplySingleTargetDamage(List<TowerStatsGA> activeTowers, List<TmpEnemyStats> tempEnemyStats)
        {
            for (var i = 0; i < activeTowers.Count; i++)
            {
                var activeTower = activeTowers[i];
                if (activeTower.IsAoe || activeTower.DamagePerSecond == 0) continue;

                for (var j = 0; j < tempEnemyStats.Count; j++)
                {
                    var enemyTempStats = tempEnemyStats[j];
                    var a = ApplySingleTargetDamage(ref enemyTempStats, ref activeTower);
                    tempEnemyStats[j] = enemyTempStats;

                    if (a) break;
                }

                activeTowers[i] = activeTower;

                DeleteDeadEnemyIndicesFromTempStats(tempEnemyStats);
            }
        }

        private bool ApplySingleTargetDamage(ref TmpEnemyStats tempEnemyStats, ref TowerStatsGA activeTower)
        {
            float timeShooting = activeTower.TimeShooting;
            var timeToKill = tempEnemyStats.Health / activeTower.DamagePerSecond;
            var maxTimeShooting = tempEnemyStats.TimeToNextWaypoint
                                  + Mathf.Clamp(tempEnemyStats.TimeToCome - timeShooting, float.MinValue, 0);

            if (maxTimeShooting <= 0) return false;

            if (timeToKill > maxTimeShooting)
            {
                tempEnemyStats.Health -= activeTower.DamagePerSecond * maxTimeShooting;
                timeShooting = Mathf.Max(timeShooting, tempEnemyStats.TimeToNextWaypoint + tempEnemyStats.TimeToCome);
                activeTower.TimeShooting = timeShooting;
                return true;
            }
            else
            {
                tempEnemyStats.Health -= activeTower.DamagePerSecond * timeToKill;

                _deadEnemiesStats.Add(tempEnemyStats);
                timeShooting = Mathf.Max(timeShooting, tempEnemyStats.TimeToCome + timeToKill);
                activeTower.TimeShooting = timeShooting;
                return false;
            }
        }

        private void DeleteDeadEnemyIndicesFromTempStats(List<TmpEnemyStats> tempEnemyStats)
        {
            foreach (var deadEnemy in _deadEnemiesStats)
            {
                tempEnemyStats.Remove(deadEnemy);
            }

            _deadEnemiesStats.Clear();
        }

        private void ApplyAoeDamage(List<TowerStatsGA> activeTowers, List<TmpEnemyStats> tempEnemyStats)
        {
            foreach (var activeTower in activeTowers)
            {
                if (!activeTower.IsAoe) continue;

                for (var i = 0; i < tempEnemyStats.Count; i++)
                {
                    var enemyTempStats = tempEnemyStats[i];
                    enemyTempStats.Health -= activeTower.DamagePerSecond * enemyTempStats.TimeToNextWaypoint;

                    tempEnemyStats[i] = enemyTempStats;

                    if (enemyTempStats.Health <= 0)
                    {
                        _deadEnemiesStats.Add(enemyTempStats);
                    }
                }

                DeleteDeadEnemyIndicesFromTempStats(tempEnemyStats);
            }
        }

        private void ApplySlowEffects(List<TowerStatsGA> activeTowers, List<TmpEnemyStats> tempEnemyStats,
            float distanceBetweenWaypoints)
        {
            foreach (var activeTower in activeTowers)
            {
                if (activeTower.SlowingFactor == 0) return;

                for (var i = 0; i < tempEnemyStats.Count; i++)
                {
                    var enemyTempStats = tempEnemyStats[i];
                    enemyTempStats.Speed *= 1 - activeTower.SlowingFactor;
                    enemyTempStats.TimeToNextWaypoint = distanceBetweenWaypoints / enemyTempStats.Speed;
                    tempEnemyStats[i] = enemyTempStats;
                }
            }
        }

        private List<TmpEnemyStats> ConvertEnemyTypesToTempStats(IEnumerable<EnemyType> enemies)
        {
            var tempEnemyStats = enemies.Select(enemyType =>
            {
                var stats = _enemyStats[enemyType];
                return new TmpEnemyStats()
                {
                    EnemyType = enemyType,
                    Health = stats.MaxHealth,
                    Speed = stats.Speed,
                    ReproductionRate = stats.ReproductionRate,
                    SpawnedType = stats.SpawnedType,
                    TimeToNextWaypoint = 0
                };
            }).ToList();
            return tempEnemyStats;
        }

        private TmpEnemyStats ConvertEnemyTypeToTempStats(EnemyType enemyType)
        {
            var stats = _enemyStats[enemyType];
            return new TmpEnemyStats()
            {
                EnemyType = enemyType,
                Health = stats.MaxHealth,
                Speed = stats.Speed,
                ReproductionRate = stats.ReproductionRate,
                SpawnedType = stats.SpawnedType,
                TimeToNextWaypoint = 0
            };
        }

        private List<TileStatsGA> CalculateTileStats(List<(Circle, TowerStatsGA)> towersInfo)
        {
            WaypointsHolder waypointsHolder = WaypointsHolderFactory.Last;
            var waypoints = waypointsHolder.Waypoints;
            Rect[] rects = ConvertToRects(waypoints);
            float distance = 1;
            var tileStatsGA = rects.Select(r => new TileStatsGA(distance, GetTowerStatsInRects(r, towersInfo)))
                .ToList();

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
                .Select(tower => tower.Item2).ToList();

            return towersInRect;
        }

        private struct TmpEnemyStats
        {
            public EnemyType EnemyType;
            public float Health;
            public float Speed;
            public float ReproductionRate;
            public EnemyType SpawnedType;
            public float TimeToNextWaypoint;
            public float TimeToCome;
        }
    }
}