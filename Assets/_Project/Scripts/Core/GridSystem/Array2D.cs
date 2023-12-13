using System;
using UnityEngine;

namespace _Project.Scripts.Core.GridSystem
{
    [Serializable]
    public class Array2D
    {
        [SerializeField] private int width;
        [SerializeField] private int height;
        [SerializeField] private bool[] gridTiles;
        
        public bool isInitialized;

        public void Initialize(int newWidth, int newHeight)
        {
            width = newWidth;
            height = newHeight;
            gridTiles = new bool[width * height];
        }
        
        private int GetIndex(int x, int y)
        {
            return y * width + x;
        }

        public bool this[int x, int y]
        {
            get => gridTiles[GetIndex(x, y)];
            set => gridTiles[GetIndex(x, y)] = value;
        }

        public bool this[Vector2Int position]
        {
            get => this[position.x, position.y];
            set => this[position.x, position.y] = value;
        }
    }
}