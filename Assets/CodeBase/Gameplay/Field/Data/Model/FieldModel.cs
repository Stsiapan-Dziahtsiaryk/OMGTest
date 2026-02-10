using Cysharp.Threading.Tasks;
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
            {
                _selectedCell = id;
                Debug.Log($"Selected cell at ({id.x}, {id.y})");
                _state.Value = FieldState.Selection;
            }
        }

        public void MoveCell(Vector2Int offset)
        {
            if (_state.Value != FieldState.Selection) return;

            var index = _selectedCell + offset;
            bool isUp = offset.y == 1 && _grid[index.x, index.y].Type == -1;

            if (CanSwap(index) == false || isUp)
                return;

            var swap = new CellDto(
                _selectedCell,
                _grid[index.x, index.y].Type,
                _grid[_selectedCell.x, _selectedCell.y].Position,
                    Cell.State.Move);
            var select = new CellDto(
                index,
                _grid[_selectedCell.x, _selectedCell.y].Type,
                _grid[index.x, index.y].Position,
                    Cell.State.Move);

            (_grid[_selectedCell.x, _selectedCell.y], _grid[index.x, index.y]) 
                = (_grid[index.x, index.y], _grid[_selectedCell.x, _selectedCell.y]); 
            
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

        // public void Normalize()
        // {
        //     _state.Value = FieldState.Normalize;
        //     Debug.Log("Start normalize.");
        //
        //     if (IsAllCellsIdle() == false) return;
        //     Debug.Log("All cells idle.");
        //     Gravity();
        //     if (_state.Value == FieldState.Gravity) return;
        //
        //     FindMatches();
        //     if (_state.Value == FieldState.Matches) return;
        //
        //     _state.Value = FieldState.Ready;
        // }

        // private void Gravity()
        // {
        //     for (int x = 0; x < Size.x; x++)
        //     {
        //         int writeY = 0;
        //         for (int y = 0; y < Size.y; y++)
        //         {
        //             if (_grid[x, y].Type != -1)
        //             {
        //                 if (y != writeY)
        //                 {
        //                     int movingType = _grid[x, y].Type;
        //
        //                     _grid[x, writeY]
        //                         .Change(new CellDto(
        //                             new Vector2Int(x, y),
        //                             movingType,
        //                             _grid[x, y].Position,
        //                             Cell.State.Move));
        //
        //                     _grid[x, y]
        //                         .Change(new CellDto(
        //                             new Vector2Int(x, y),
        //                             -1,
        //                             _grid[x, writeY].Position,
        //                             Cell.State.Idle));
        //                     _state.Value = FieldState.Gravity;
        //                 }
        //
        //                 writeY++;
        //             }
        //         }
        //     }
        // }
        //
        // private void FindMatches()
        // {
        //     bool[,] toDestroy = new bool[Size.x, Size.y];
        //
        //     for (int y = 0; y < Size.y; y++)
        //     {
        //         int runStart = 0;
        //         while (runStart < Size.x)
        //         {
        //             int t = _grid[runStart, y].Type;
        //             int runEnd = runStart + 1;
        //             while (runEnd < Size.x &&
        //                    _grid[runEnd, y].Type == t &&
        //                    t != -1)
        //                 runEnd++;
        //
        //             int runLength = runEnd - runStart;
        //             if (t != -1 && runLength >= 3)
        //             {
        //                 for (int x = runStart; x < runEnd; x++)
        //                     toDestroy[x, y] = true;
        //             }
        //
        //             runStart = runEnd;
        //         }
        //     }
        //
        //     for (int x = 0; x < Size.x; x++)
        //     {
        //         int runStart = 0;
        //         while (runStart < Size.y)
        //         {
        //             int t = _grid[x, runStart].Type;
        //             int runEnd = runStart + 1;
        //             while (runEnd < Size.y &&
        //                    _grid[x, runEnd].Type == t &&
        //                    t != -1)
        //                 runEnd++;
        //
        //             int runLength = runEnd - runStart;
        //             if (t != -1 && runLength >= 3)
        //             {
        //                 for (int y = runStart; y < runEnd; y++)
        //                     toDestroy[x, y] = true;
        //             }
        //
        //             runStart = runEnd;
        //         }
        //     }
        //
        //     for (int y = 0; y < Size.y; y++)
        //     {
        //         for (int x = 0; x < Size.x; x++)
        //         {
        //             if (toDestroy[x, y])
        //             {
        //                 _state.Value = FieldState.Matches;
        //                 Debug.Log($"Destroying cell at ({x}, {y})");
        //                 _grid[x, y].Change(new CellDto(
        //                     new Vector2Int(x, y),
        //                     -1,
        //                     _grid[x, y].Position,
        //                     Cell.State.Destroy));
        //             }
        //         }
        //     }
        // }

        
        public async UniTask NormalizeAsync()
        {
            _state.Value = FieldState.Normalize;
            Debug.Log("Start normalize.");

            do
            {
                if (!IsAllCellsIdle())
                    await UniTask.WaitUntil(IsAllCellsIdle);

                bool moved = await GravityAsync();
                if (moved)
                    await UniTask.WaitUntil(IsAllCellsIdle);

                bool destroyed = await FindMatchesAsync();
                if (destroyed)
                    await UniTask.WaitUntil(IsAllCellsIdle);

                // Повторяем, пока что-то происходит
                if (!moved && !destroyed)
                    break;
            }
            while (true);

            _state.Value = FieldState.Ready;
        }
        
        private async UniTask<bool> GravityAsync()
        {
            bool moved = false;

            for (int x = 0; x < Size.x; x++)
            {
                int writeY = 0;
                for (int y = 0; y < Size.y; y++)
                {
                    if (_grid[x, y].Type != -1)
                    {
                        if (y != writeY) // y = 1, writeY = 0/ y = 2, writeY = 1 
                        {
                            (_grid[x, writeY], _grid[x, y]) =
                                (_grid[x, y], _grid[x, writeY]);

                            var dropping = new CellDto(
                                new Vector2Int(x, writeY),
                                _grid[x, writeY].Type,
                                _grid[x, y].Position,
                                Cell.State.Move
                            );

                            var upping = new CellDto(
                                new Vector2Int(x, y),
                                _grid[x, y].Type,
                                _grid[x, writeY].Position,
                                Cell.State.Move
                            );
                            
                            _grid[x, writeY].Change(dropping);

                            _grid[x, y].Change(upping);
                            
                            moved = true;
                        }

                        writeY++;
                    }
                }
            }

            if (moved)
            {
                _state.Value = FieldState.Gravity;
                // Отдать кадр(ы) системе анимации/презентеру
                await UniTask.Yield(PlayerLoopTiming.Update);
            }

            return moved;
        }
        
         private async UniTask<bool> FindMatchesAsync()
        {
            bool[,] toDestroy = new bool[Size.x, Size.y];
            bool anyDestroyed = false;

            // Горизонтальные серии
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

            // Вертикальные серии
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

            // Применяем уничтожение
            for (int y = 0; y < Size.y; y++)
            {
                for (int x = 0; x < Size.x; x++)
                {
                    if (toDestroy[x, y])
                    {
                        anyDestroyed = true;
                        _grid[x, y].Change(new CellDto(
                            new Vector2Int(x, y),
                            -1,
                            _grid[x, y].Position,
                            Cell.State.Destroy));
                    }
                }
            }

            if (anyDestroyed)
            {
                _state.Value = FieldState.Matches;
                // Отдать кадр(ы) системе анимации/презентеру
                await UniTask.Yield(PlayerLoopTiming.Update);
            }

            return anyDestroyed;
        }
        
        private bool IsAllCellsIdle()
        {
            bool isIdle = false;

            foreach (var cell in _grid)
            {
                isIdle = cell.CurrentState == Cell.State.Idle;
                if (isIdle == false) break;
            }

            return isIdle;
        }
    }
}