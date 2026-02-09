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

            NormalizeGrid();
        }

        public Cell GetCell(int x, int y) => _grid[x, y];

        public void ChangeState(FieldState state)
            => _state.Value = state;

        
        public void OnDoNextCellAction()
        {
            foreach (Cell cell in _grid)
            {
                cell.NextAction();
            }
        }
        
        private bool CanSwap(Vector2Int swapTarget)
        {
            bool isValid =
                swapTarget.x >= 0 &&
                swapTarget.x < Size.x &&
                swapTarget.y >= 0 &&
                swapTarget.y < Size.y;

            return isValid;
        }
        
        private void Gravity()
        {
            for (int x = 0; x < Size.x; x++)
            {
                int writeY = 0;
                for (int y = 0; y < Size.y; y++)
                {
                    if (_grid[x, y].Type != -1)
                    {
                        if (y != writeY)
                        {
                            int movingType = _grid[x, y].Type;

                            _grid[x, writeY].Change(new CellDto(
                                movingType,
                                _grid[x, writeY].Position,
                                Cell.State.Move));

                            _grid[x, y].Change(new CellDto(
                                -1,
                                _grid[x, y].Position,
                                Cell.State.Move));
                        }

                        writeY++;
                    }
                }
            }
        }

        private void NormalizeGrid()
        {
            _state.Value = FieldState.Normalize;
            Gravity();
            
            bool anyDestroyed;
            do
            {
                bool[,] toDestroy = new bool[Size.x, Size.y];
                anyDestroyed = false;

                for (int y = 0; y < Size.y; y++)
                {
                    int runStart = 0;
                    while (runStart < Size.x)
                    {
                        int t = _grid[runStart, y].Type;
                        int runEnd = runStart + 1;
                        while (runEnd < Size.x && _grid[runEnd, y].Type == t && t != -1)
                            runEnd++;

                        int runLength = runEnd - runStart;
                        if (t != -1 && runLength >= 3)
                        {
                            for (int x = runStart; x < runEnd; x++)
                                toDestroy[x, y] = true;
                        }

                        runStart = runEnd;
                    }
                }

                for (int x = 0; x < Size.x; x++)
                {
                    int runStart = 0;
                    while (runStart < Size.y)
                    {
                        int t = _grid[x, runStart].Type;
                        int runEnd = runStart + 1;
                        while (runEnd < Size.y && _grid[x, runEnd].Type == t && t != -1)
                            runEnd++;

                        int runLength = runEnd - runStart;
                        if (t != -1 && runLength >= 3)
                        {
                            for (int y = runStart; y < runEnd; y++)
                                toDestroy[x, y] = true;
                        }

                        runStart = runEnd;
                    }
                }

                for (int y = 0; y < Size.y; y++)
                {
                    for (int x = 0; x < Size.x; x++)
                    {
                        if (toDestroy[x, y])
                        {
                            anyDestroyed = true;
                            _grid[x, y].Change(new CellDto(
                                -1,
                                _grid[x, y].Position,
                                Cell.State.Destroy));
                        }
                    }
                }

                if (anyDestroyed)
                {
                    Gravity();
                }
            } while (anyDestroyed);

            _state.Value = FieldState.Ready;
        }
    }
}