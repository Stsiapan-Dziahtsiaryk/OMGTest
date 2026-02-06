using System;
using UnityEngine;

namespace CodeBase.Gameplay.Field.Config
{
    [CreateAssetMenu(fileName = "Level", menuName = "Data/Configs/Create Level", order = 51)]
    public class LevelConfig : ScriptableObject
    {
        [SerializeField] private int _id;
        [SerializeField] private Vector2Int _gridSize = new Vector2Int(2,2);
        [SerializeField] private BlockConfig[] _blocks;
        [SerializeField] private int[] _grid;
        
        public int ID => _id;
        public Vector2Int GridSize => _gridSize;
        public BlockConfig[] Blocks => _blocks;
        public int[] Grid => _grid;
    }
}