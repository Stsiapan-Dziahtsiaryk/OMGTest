using System;
using CodeBase.Gameplay.Controller;
using CodeBase.Gameplay.MVP;
using R3;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace CodeBase.Gameplay.Field
{
    public class FieldPresenter : PresenterBase<FieldView>
    {
        private readonly FieldModel _model;
        private readonly CellView.Pool _cellPool;
        private readonly InputService _inputService;
        
        private Camera _camera;
        private CellView _selectedCell;
        
        public FieldPresenter(
            FieldView view,
            FieldModel model, 
            CellView.Pool cellPool,
            InputService inputService) : base(view)
        {
            _model = model ?? throw new ArgumentNullException(nameof(model));
            _cellPool = cellPool ?? throw new ArgumentNullException(nameof(cellPool));
            _inputService = inputService ?? throw new ArgumentNullException(nameof(inputService));
            _camera = Camera.main;
        }

        protected override void OnAttach()
        {
            _model.State.Subscribe(OnHandleState).AddTo(View);
            _inputService.PickEvent += OnSelectCell;
            _inputService.Swiped += OnSwipe;
        }

        protected override void OnDetach()
        {
            base.OnDetach();
            _inputService.PickEvent -= OnSelectCell;
            _inputService.Swiped -= OnSwipe;
        }
        
        private void OnSwipe(int direction)
        {
            Debug.Log($"Swipe direction: {direction}");
            Vector2Int offset = Vector2Int.zero;
            switch (direction)
            {
                case 1: // Up
                    offset = new Vector2Int(0, 1);
                    break;
                case 2: // Down
                    offset = new Vector2Int(0, -1);
                    break;
                case 3: // Left
                    offset = new Vector2Int(-1, 0);
                    break;
                case 4: // Right
                    offset = new Vector2Int(1, 0);
                    break;
            }
            _model.MoveCell(offset);
        }

        private void OnSelectCell(Vector3 point)
        {
            Vector2 worldPosition = _camera.ScreenToWorldPoint(point);
            RaycastHit2D hit = Physics2D.Raycast(
                worldPosition, 
                Vector2.zero,
                Mathf.Infinity, 1 << 6);
            if (hit.collider != null 
                && hit.collider.TryGetComponent(out CellView cell))
            {
                cell.Selected();
            }
        }

        private void OnHandleState(FieldState state)
        {
            switch (state)
            {
                case FieldState.Invalid:
                    break;
                case FieldState.Ready:
                    break;
                case FieldState.Building:
                    RebuildGrid();
                    break;
                case FieldState.Normalize:
                    break;
                case FieldState.Rebuilding:
                    ClearGrid();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }
        
        private void RebuildGrid()
        {
            Vector3 scale = new Vector3(_model.Scale, _model.Scale, 1);

            for (int y = 0; y < _model.Size.y; y++)
            {
                for (int x = 0; x < _model.Size.x; x++)
                {
                    Cell cell = _model.GetCell(x, y);
    
                    var view = _cellPool.Spawn();
                    view.transform.SetParent(View.transform, false);
                    view.transform.localPosition = cell.Position;
                    view.transform.localScale = scale;

                    view.SetBlock(new Vector2Int(x,y), cell.Type);
                    view.Selecting += _model.OnSelectCell;
                    cell.Changed += view.HandleState;
                }
            }
            _model.ChangeState(FieldState.Ready);
        }
        
        private void ClearGrid()
        {
            _cellPool.DespawnAll();
        }
    }
}