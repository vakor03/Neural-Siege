using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Project.Scripts.Core.GridSystem
{
    public class PathFactory
    {
        public List<Vector3Int> Create(Vector3Int start, Vector3Int end, List<Vector3Int> points)
        {
            List<Vector3Int> path = new List<Vector3Int>(points.Count + 2);
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
        
        private Vector3Int GetNeighbourButNotPrevious(Vector3Int position, Vector3Int previous, List<Vector3Int> points)
        {
            var neighbours = GetNeighbours(position);
            return neighbours.First(n => n != previous && points.Contains(n));
        }

        private Vector3Int GetNeighbour(Vector3Int position, List<Vector3Int> points)
        {
            var neighbours = GetNeighbours(position);
            return neighbours.First(points.Contains);
        }

        private IEnumerable<Vector3Int> GetNeighbours(Vector3Int position)
        {
            yield return position + Vector3Int.up;
            yield return position + Vector3Int.down;
            yield return position + Vector3Int.left;
            yield return position + Vector3Int.right;
        }
    }

    public class PathValidator
    {
        public bool ValidatePath(Vector3Int start, Vector3Int end, List<Vector3Int> path)
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
        
        private int CountNeighbours(Vector3Int position, List<Vector3Int> path)
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

        private IEnumerable<Vector3Int> GetNeighbours(Vector3Int position)
        {
            yield return position + Vector3Int.up;
            yield return position + Vector3Int.down;
            yield return position + Vector3Int.left;
            yield return position + Vector3Int.right;
        }
    }
}