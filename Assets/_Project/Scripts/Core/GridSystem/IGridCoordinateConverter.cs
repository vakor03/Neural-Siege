using UnityEngine;

namespace _Project.Scripts.Core.GridSystem
{
    public interface IGridCoordinateConverter
    {
        Vector2Int WorldToGrid(Vector3 worldPosition, float cellSize, Vector3 originPosition);
        Vector3 GridToWorld(Vector2Int gridPosition, float cellSize, Vector3 originPosition);
        Vector3 GridToWorldCentered(Vector2Int gridPosition, float cellSize, Vector3 originPosition);
        Vector3 Forward { get; }
    }
}