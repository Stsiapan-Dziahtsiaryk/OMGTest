using System.Collections.Generic;
using R3;
using UnityEngine;

namespace CodeBase.Gameplay.Field
{
    public class FieldModel
    {
        private readonly ReactiveProperty<FieldState> _state;

        private float _cellScale;
        private Cell[,] _grid;

        private Vector2Int _selectedCell;

        public FieldModel()
        {
            _state = new ReactiveProperty<FieldState>(FieldState.Ready);
        }

        public ReadOnlyReactiveProperty<FieldState> State => _state;
        public float Scale => _cellScale;
        public Vector2Int Size { get; private set; }

        public void Initialize(float scale, Cell[,] grid)
        {
            _cellScale = scale;
            Size = new Vector2Int(grid.GetLength(0), grid.GetLength(1));
            _grid = grid;
            _state.Value = FieldState.Building;
        }

        public void OnSelectCell(Vector2Int id)
        {
            if (_grid[id.x, id.y].Type != -1)
                _selectedCell = id;
        }

        public void MoveCell(Vector2Int offset)
        {
            var index = _selectedCell + offset;
            bool isUp = offset.y == 1 && _grid[index.x, index.y].Type == -1;
            
            if (CanSwap(index) == false || isUp)
                return;

            var swap = new CellDto(
                _grid[index.x, index.y].Type,
                _grid[index.x, index.y].Position,
                Cell.State.Move);
            var select = new CellDto(
                _grid[_selectedCell.x, _selectedCell.y].Type,
                _grid[_selectedCell.x, _selectedCell.y].Position,
                Cell.State.Move);

            _grid[_selectedCell.x, _selectedCell.y].Change(swap);
            _grid[index.x, index.y].Change(select);
        }

        public Cell GetCell(int x, int y) => _grid[x, y];

        public void ChangeState(FieldState state)
            => _state.Value = state;
        
        private bool CanSwap(Vector2Int swapTarget)
        {
            bool isValid =
                swapTarget.x >= 0 &&
                swapTarget.x < Size.x &&
                swapTarget.y >= 0 &&
                swapTarget.y < Size.y;

            return isValid;
        }
    }
}