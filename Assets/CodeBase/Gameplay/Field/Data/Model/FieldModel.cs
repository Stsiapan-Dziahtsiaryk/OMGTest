using System.Collections.Generic;
using R3;
using UnityEngine;

namespace CodeBase.Gameplay.Field
{
    public class FieldModel
    {
        private readonly ReactiveProperty<FieldState> _state;
        
        private float _cellScale;
        private Cell[] _grid;

        public FieldModel()
        {
            _state = new ReactiveProperty<FieldState>(FieldState.Ready);
        }
        
        public IReadOnlyList<Cell> Grid => _grid;
        
        public ReadOnlyReactiveProperty<FieldState> State => _state;
        public float Scale => _cellScale;

        public void Initialize(float scale, Cell[] grid)
        {
            _cellScale = scale;
            _grid = grid;
            _state.Value = FieldState.Building;
        }
        
        public void ChangeState(FieldState state) 
            => _state.Value = state;
    }
    
    // состояния поля 
    // готово к игре
    // нормализация 
}