using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace _Project.Scripts.Core.GridSystem
{
    public class GridData
    {
        private Dictionary<Vector3Int, GameObject> _placedObjects = new();

        public bool IsPlacementValid(PlacementSystemObjectSO activeSO, Vector3Int zeroPosition)
        {
            Vector2Int size = activeSO.size;
            for (int i = 0; i < size.y; i++)
            {
                for (int j = 0; j < size.x; j++)
                {
                    if (_placedObjects.ContainsKey(zeroPosition + new Vector3Int(j, i, 0)))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public void AddObject(PlacementSystemObjectSO objectSO, Vector3Int gridPosition, GameObject instance)
        {
            for (int i = 0; i < objectSO.size.y; i++)
            {
                for (int j = 0; j < objectSO.size.x; j++)
                {
                    _placedObjects.Add(gridPosition + new Vector3Int(j, i, 0), instance);
                }
            }
        }

        [CanBeNull]
        public GameObject GetObject(Vector3Int gridPosition)
        {
            return _placedObjects.GetValueOrDefault(gridPosition);
        }

        public void RemoveObject(Vector3Int gridPosition)
        {
            var placedObject = _placedObjects[gridPosition];

            List<Vector3Int> keysToRemove = new();
            foreach (var (key, value) in _placedObjects)
            {
                if (value == placedObject)
                {
                    keysToRemove.Add(key);
                }
            }

            foreach (var key in keysToRemove)
            {
                _placedObjects.Remove(key);
            }
        }
    }
}