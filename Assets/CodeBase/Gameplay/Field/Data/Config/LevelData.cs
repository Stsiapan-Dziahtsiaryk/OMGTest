using UnityEngine;

namespace CodeBase.Gameplay.Field.Config
{
    public readonly struct LevelData
    {
        public readonly int ID;
        public readonly Vector2Int GridSize;
        public readonly int[] Grid;

        public LevelData(
            int id,
            Vector2Int gridSize,
            int[] grid)
        {
            ID = id;
            GridSize = gridSize;
            Grid = grid;
        }
    }
}