using UnityEngine;

namespace _Project.Scripts.Core.GridSystem
{
    public class HorizontalConverter : IGridCoordinateConverter
    {
        public Vector2Int WorldToGrid(Vector3 worldPosition, float cellSize, Vector3 originPosition)
        {
            Vector3 gridPosition = (worldPosition - originPosition) / cellSize;
            var x = Mathf.FloorToInt(gridPosition.x);
            var y = Mathf.FloorToInt(gridPosition.y);
            return new Vector2Int(x, y);
        }

        public Vector3 GridToWorld(Vector2Int gridPosition, float cellSize, Vector3 originPosition)
        {
            return new Vector3(gridPosition.x * cellSize, gridPosition.y * cellSize, 0)
                   + originPosition;
        }

        public Vector3 GridToWorldCentered(Vector2Int gridPosition, float cellSize, Vector3 originPosition)
        {
            return new Vector3(gridPosition.x * cellSize + cellSize/2, gridPosition.y * cellSize+ cellSize/2, 0)
                   + originPosition;
        }

        public Vector3 Forward => Vector3.forward;
    }
}