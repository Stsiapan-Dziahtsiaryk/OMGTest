using System;
using UnityEngine;

namespace CodeBase.Gameplay.Field
{
    public class Cell
    {
        public enum State
        {
            Invalid = 0,
            Idle = 1,
            Move = 2,
            Destroy = 3,
        }
        
        private State _state = State.Idle;
        private CellDto _cashedAction;
        
        public Cell(int type, Vector2 position)
        {
            Type = type;
            Position = position;
        }

        public int Type { get; private set; }
        public Vector2 Position { get; private set; }
        public State CurrentState => _state;
        public event Action<CellDto> Changed;
        
        public void Change(CellDto data)
        {
            Type = data.Type;
            Position = data.Position;
            _state = data.State;
            Changed?.Invoke(data);
        }
        
        public void SetState()
        {
            _state = State.Idle;
        }
    }
}