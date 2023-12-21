using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using _Project.Scripts.Algorithms;
using _Project.Scripts.Algorithms.GA;
using _Project.Scripts.Algorithms.GA.Structs;
using _Project.Scripts.Core.Enemies;
using _Project.Scripts.Core.GridSystem;
using _Project.Scripts.Core.Towers;
using _Project.Scripts.Core.WaypointSystem;
using KBCore.Refs;
using UnityEngine;
using Zenject;

public class TestGA : ValidatedMonoBehaviour
{
    [SerializeField] private int enemiesPerWave = 10;
    [SerializeField, Scene] private EnemySpawner enemySpawner;

    private IWaveCreationAlgorithm _algorithm;
    private TowersController _towersController;

    [Inject]
    private void Construct(GeneticAlgorithmFactory geneticAlgorithmFactory,
        TowersController towersController)
    {
        _towersController = towersController;
        _algorithm = geneticAlgorithmFactory.Create();
    }

    [ContextMenu("Create and Spawn Wave")]
    public void CreateAndSpawnWave()
    {
        List<(Circle, TowerStatsGA)> towersInfo = _towersController.Towers
            .Select(tower => (new Circle(tower.transform.position, tower.Range), tower.GetTowerStatsGA()))
            .ToList();
        
        var tileStats = CalculateTileStats(towersInfo);
        var wave = _algorithm.CreateEnemyWave(tileStats, enemiesPerWave);
        enemySpawner.SpawnWave(wave);
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

public struct Circle
{
    public Vector2 Center { get; set; }
    public float Radius { get; set; }

    public Circle(Vector2 center, float radius)
    {
        Center = center;
        Radius = radius;
    }

    public bool Intersects(Rect rect)
    {
        float closestX = Mathf.Clamp(Center.x, rect.x, rect.x + rect.width);
        float closestY = Mathf.Clamp(Center.y, rect.y, rect.y + rect.height);

        Vector2 closestPoint = new Vector2(closestX, closestY);
        float distance = Vector2.Distance(Center, closestPoint);

        return distance < Radius;
    }
}