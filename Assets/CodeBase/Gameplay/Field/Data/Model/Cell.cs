using UnityEngine;

namespace CodeBase.Gameplay.Field
{
    public class Cell
    {
        public Cell(int id, int type, Vector2 position)
        {
            ID = id;
            Type = type;
            Position = position;
        }

        public int ID { get; private set; }
        public int Type { get; private set; }
        public Vector2 Position { get; private set; }
    }
    
    // состояния клетки 
    // пустая 
    // занята 
    
}