using System;
using System.Collections.Generic;
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
        
        private Queue<CellDto> _actionQueue = new Queue<CellDto>();
        
        public Cell(int type, Vector2 position)
        {
            Type = type;
            Position = position;
        }

        public int Type { get; private set; }
        public Vector2 Position { get; private set; }

        public event Action<CellDto> Changed;

        public void SetAction(CellDto data)
        {
            _actionQueue.Enqueue(data);
        }
        
        public void Change(CellDto data)
        {
            Type = data.Type;
            Position = data.Position;
            Changed?.Invoke(data);
        }

        public void NextAction()
        {
            if (_actionQueue.Count == 0) return;
            var action = _actionQueue.Dequeue();
            Change(action);
        }
    }
}