using UnityEngine;

namespace CodeBase.Gameplay.Field
{
    public class Cell
    {
        public Cell(int type, Vector2 position)
        {
            Type = type;
            Position = position;
        }

        public int Type { get; private set; }
        public Vector2 Position { get; private set; }
    }
    
    // состояния клетки 
    // пустая 
    // занята 
    
}