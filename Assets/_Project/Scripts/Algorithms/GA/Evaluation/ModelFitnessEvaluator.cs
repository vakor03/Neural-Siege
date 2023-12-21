using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Algorithms.GA.Chromosomes;
using _Project.Scripts.Algorithms.GA.Structs;
using _Project.Scripts.Core.Enemies;
using _Project.Scripts.Core.Towers;

namespace _Project.Scripts.Algorithms.GA.Evaluation
{
    public class ModelFitnessEvaluator : IModelFitnessEvaluator
    {
        private Dictionary<EnemyType, EnemyStatsGA> _enemyStats;
        // private Dictionary<TowerTypeSO, TowerStatsGA> _towerStats;
        public List<TileStatsGA> TilesStats { get; set; } = new();

        private readonly HashSet<TmpEnemyStats>
            _deadEnemiesStats = new(); // This can work not as expected cause of struct

        private const float PER_ENEMY_REACHED_COEFFICIENT = 1000f;
        private const float WAYPOINT_REACHED_COEFFICIENT = 100f;
        private const float PRICE_COEFFICIENT = -1f;

        public ModelFitnessEvaluator(Dictionary<EnemyType, EnemyStatsGA> enemyStats)
        {
            _enemyStats = enemyStats;
        }

        public float Evaluate(Chromosome chromosome)
        {
            float survivalScore = CalculateSurvivalScore(chromosome);

            return survivalScore;
        }

        private float CalculateSurvivalScore(Chromosome chromosome)
        {
            var survived = ConvertEnemyTypesToTempStats(chromosome.EnemyWave);
            int lastReachedWaypoint = 0;

            for (var i = 0; i < TilesStats.Count; i++)
            {
                if (survived.Count == 0) break;

                var tilesStat = TilesStats[i];
                survived = CalculateWhoWillSurviveOnTile(survived, tilesStat);
                lastReachedWaypoint = i;
            }

            float value;
            float totalPrice = chromosome.EnemyWave.Sum(el => _enemyStats[el].Price);
            if (lastReachedWaypoint == TilesStats.Count - 1)
            {
                value = survived.Count * PER_ENEMY_REACHED_COEFFICIENT +
                        lastReachedWaypoint * WAYPOINT_REACHED_COEFFICIENT +
                        totalPrice * PRICE_COEFFICIENT;
            }
            else
            {
                value = lastReachedWaypoint * WAYPOINT_REACHED_COEFFICIENT +
                        totalPrice * PRICE_COEFFICIENT;
            }

            return value;
        }

        private List<TmpEnemyStats> CalculateWhoWillSurviveOnTile(List<TmpEnemyStats> tempEnemyStats,
            TileStatsGA tileStatsGA)
        {
            UpdateTimeToNextWaypoint(tempEnemyStats, tileStatsGA.Distance);
            var activeTowers = tileStatsGA.Towers;
            // TODO: take into account order and time between enemies
            // TODO: take spawn rate into account

            ApplySlowEffects(activeTowers, tempEnemyStats, tileStatsGA.Distance);

            ApplyAoeDamage(activeTowers, tempEnemyStats);

            ApplySingleTargetDamage(activeTowers, tempEnemyStats);

            return tempEnemyStats.OrderBy(tmp => tmp.TimeToNextWaypoint).ToList();
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
            foreach (var activeTower in activeTowers)
            {
                if (activeTower.IsAoe || activeTower.DamagePerSecond == 0) continue;
                float timeShooting = 0;

                for (var i = 0; i < tempEnemyStats.Count; i++)
                {
                    var enemyTempStats = tempEnemyStats[i];
                    ApplySingleTargetDamage(ref enemyTempStats, activeTower, ref timeShooting);
                    tempEnemyStats[i] = enemyTempStats;
                }

                DeleteDeadEnemyIndicesFromTempStats(tempEnemyStats);
            }
        }

        private void ApplySingleTargetDamage(ref TmpEnemyStats tempEnemyStats, TowerStatsGA activeTower,
            ref float timeShooting)
        {
            var timeToKill = tempEnemyStats.Health / activeTower.DamagePerSecond;

            if (timeToKill > tempEnemyStats.TimeToNextWaypoint - timeShooting)
            {
                tempEnemyStats.Health -= activeTower.DamagePerSecond *
                                         (tempEnemyStats.TimeToNextWaypoint - timeShooting);

                timeShooting = tempEnemyStats.TimeToNextWaypoint;
            }
            else
            {
                tempEnemyStats.Health -= activeTower.DamagePerSecond *
                                         (tempEnemyStats.TimeToNextWaypoint - timeShooting);

                _deadEnemiesStats.Add(tempEnemyStats);
                timeShooting += timeToKill;
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

        private List<TmpEnemyStats> ConvertEnemyTypesToTempStats(EnemyType[] enemies)
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

        private struct TmpEnemyStats
        {
            public EnemyType EnemyType;
            public float Health;
            public float Speed;
            public float ReproductionRate;
            public EnemyType SpawnedType;
            public float TimeToNextWaypoint;
        }
    }
}