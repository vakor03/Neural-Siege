using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Project.Scripts.Core.GridSystem
{
    public class PathFactory
    {
        public List<Vector2Int> Create(Vector2Int start, Vector2Int end, List<Vector2Int> points)
        {
            List<Vector2Int> path = new List<Vector2Int>(points.Count + 2);
            var previous = start;
            var current = GetNeighbour(start, points);
            path.Add(previous);
            path.Add(current);
            for (var i = 1; i < points.Count; i++)
            {
                var point = GetNeighbourButNotPrevious(current, previous, points);
                previous = current;
                current = point;
                path.Add(current);
            }
            path.Add(end);
            return path;
        }
        
        private Vector2Int GetNeighbourButNotPrevious(Vector2Int position, Vector2Int previous, List<Vector2Int> points)
        {
            var neighbours = GetNeighbours(position);
            return neighbours.First(n => n != previous && points.Contains(n));
        }

        private Vector2Int GetNeighbour(Vector2Int position, List<Vector2Int> points)
        {
            var neighbours = GetNeighbours(position);
            return neighbours.First(points.Contains);
        }

        private IEnumerable<Vector2Int> GetNeighbours(Vector2Int position)
        {
            yield return position + Vector2Int.up;
            yield return position + Vector2Int.down;
            yield return position + Vector2Int.left;
            yield return position + Vector2Int.right;
        }
    }

    public class PathValidator
    {
        public bool ValidatePath(Vector2Int start, Vector2Int end, List<Vector2Int> path)
        {
            var fullList = path.Concat(new []{start, end}).ToList();
            
            var startNeighbours = CountNeighbours(start, fullList);
            var endNeighbours = CountNeighbours(end, fullList);
            if (startNeighbours != 1 || endNeighbours != 1)
            {
                return false;
            }
            
            foreach (var position in path)
            {
                var neighbours = CountNeighbours(position, fullList);
                if (neighbours != 2)
                {
                    return false;
                }
            }

            return true;
        }
        
        private int CountNeighbours(Vector2Int position, List<Vector2Int> path)
        {
            int count = 0;
            foreach (var neighbour in GetNeighbours(position))
            {
                if (path.Contains(neighbour))
                {
                    count++;
                }
            }

            return count;
        }

        private IEnumerable<Vector2Int> GetNeighbours(Vector2Int position)
        {
            yield return position + Vector2Int.up;
            yield return position + Vector2Int.down;
            yield return position + Vector2Int.left;
            yield return position + Vector2Int.right;
        }
    }
}