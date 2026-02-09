using System;
using UnityEngine;

namespace CodeBase.Gameplay.Field
{
    public class Cell
    {
        public enum State
        {
            Invalid = 0,
            Move = 1,
            Destroy = 2,
        }
        
        public Cell(int type, Vector2 position)
        {
            Type = type;
            Position = position;
        }

        public int Type { get; private set; }
        public Vector2 Position { get; private set; }

        public event Action<CellDto> Changed;

        public void Change(CellDto data)
        {
            Type = data.Type;
            Position = data.Position;
            Changed?.Invoke(data);
        }
    }
}