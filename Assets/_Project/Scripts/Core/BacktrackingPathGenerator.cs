using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Core
{
    public class BacktrackingPathGenerator
    {
        public List<Vector2Int> GeneratePath(Vector2Int size, Vector2Int start, Vector2Int end)
        {
            bool[,] grid = new bool[size.x, size.y];
            grid[start.x, start.y] = true;

            var path = new List<Vector2Int>();
            if (TryGeneratePathWithBacktracking(grid, size, end, start, path))
            {
                path.Add(start);
                path.Reverse();
                return path;
            }

            Debug.LogError("Path not found");
            return null;
        }

        private bool TryGeneratePathWithBacktracking(bool[,] grid, Vector2Int size, Vector2Int end, Vector2Int current,
            List<Vector2Int> path)
        {
            if (current == end)
            {
                return true;
            }

            Vector2Int[] directions = { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };
            Shuffle(directions);

            foreach (Vector2Int direction in directions)
            {
                Vector2Int next = current + direction;
                if (IsPositionValid(grid, size, next, current))
                {
                    grid[next.x, next.y] = true;
                    if (TryGeneratePathWithBacktracking(grid, size, end, next, path))
                    {
                        path.Add(next);
                        return true;
                    }

                    grid[next.x, next.y] = false;
                }
            }

            return false;
        }

        private bool IsPositionValid(bool[,] grid, Vector2Int size, Vector2Int position, Vector2Int previous)
        {
            return CheckPositionValid() && CheckNearest(position, grid, size, previous);

            bool CheckPositionValid()
            {
                return position.x >= 0 && position.x < size.x && position.y >= 0 && position.y < size.y &&
                       !grid[position.x, position.y];
            }
        }

        private bool CheckNearest(Vector2Int position, bool[,] grid, Vector2Int size, Vector2Int previous)
        {
            return CheckDirection(previous, position + Vector2Int.up, grid, size) &&
                   CheckDirection(previous, position + Vector2Int.right, grid, size) &&
                   CheckDirection(previous, position + Vector2Int.down, grid, size) &&
                   CheckDirection(previous, position + Vector2Int.left, grid, size);
        }

        private bool CheckDirection(Vector2Int previous, Vector2Int current, bool[,] grid, Vector2Int size)
        {
            if (previous == current)
            {
                return true;
            }

            if (current.x < 0 || current.x >= size.x || current.y < 0 || current.y >= size.y)
            {
                return true;
            }

            if (grid[current.x, current.y])
            {
                return false;
            }

            if (grid[current.x, current.y])
            {
                return false;
            }

            return true;
        }

        private static void Shuffle(Vector2Int[] array)
        {
            for (int i = array.Length - 1; i > 0; i--)
            {
                int randomIndex = Random.Range(0, i + 1);

                (array[i], array[randomIndex]) = (array[randomIndex], array[i]);
            }
        }
    }
}