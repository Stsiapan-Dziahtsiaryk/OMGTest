using UnityEngine;

namespace CodeBase.Gameplay.Field
{
    public readonly struct CellDto
    {
        // Какой тип блока после перемещения будет
        public readonly int Type;
        // В какую сторону перемещение 
        public readonly Vector2 Position;
        public readonly Cell.State State;
        
        public CellDto(
            int type,
            Vector2 position,
            Cell.State state)
        {
            Type = type;
            Position = position;
            State = state;
        }
    }
}