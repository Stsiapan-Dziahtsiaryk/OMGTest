using System;
using CodeBase.Gameplay.Controller;
using CodeBase.Gameplay.Level;
using CodeBase.Gameplay.MVP;
using Cysharp.Threading.Tasks;
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

        private readonly GameSettings _gameSettings;
        
        private Camera _camera;
        private CellView _selectedCell;
        private UniTask _runningNormalize = UniTask.CompletedTask;

        public FieldPresenter(
            FieldView view,
            FieldModel model, 
            CellView.Pool cellPool,
            InputService inputService,
            GameSettings gameSettings) : base(view)
        {
            _model = model ?? throw new ArgumentNullException(nameof(model));
            _cellPool = cellPool ?? throw new ArgumentNullException(nameof(cellPool));
            _inputService = inputService ?? throw new ArgumentNullException(nameof(inputService));
            _gameSettings = gameSettings ?? throw new ArgumentNullException(nameof(gameSettings));
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
            Vector2Int offset = Vector2Int.zero;
            switch (direction)
            {
                case 1: // Up
                    offset = Vector2Int.up;
                    break;
                case 2: // Down
                    offset = Vector2Int.down;
                    break;
                case 3: // Left
                    offset = Vector2Int.left;
                    break;
                case 4: // Right
                    offset = Vector2Int.right;
                    break;
            }
            _model.MoveCell(offset);
            
            if (_runningNormalize.Status.IsCompleted())
                _runningNormalize = _model.NormalizeAsync().AttachExternalCancellation(View.GetCancellationTokenOnDestroy());

        }

        private void OnSelectCell(Vector3 point)
        {
            if(_model.State.CurrentValue != FieldState.Ready) return;
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
            if (state == FieldState.Building)
                    BuildGrid();

            if (state == FieldState.Rebuilding)
                    ClearGrid();
        }
        
        private void BuildGrid()
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

                    AnimationClip[] clips = null;
                    clips = cell.Type == -1 ? null : _gameSettings.BlockConfigs[cell.Type].Animations;
                        
                    
                    view.Initialize(new Vector2Int(x, y), clips);
                    view.Selecting += _model.OnSelectCell;
                    view.Callback += cell.SetState;
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