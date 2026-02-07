using System;
using System.Numerics;
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
        
        public FieldPresenter(
            FieldView view,
            FieldModel model, 
            CellView.Pool cellPool) : base(view)
        {
            _model = model ?? throw new ArgumentNullException(nameof(model));
            _cellPool = cellPool ?? throw new ArgumentNullException(nameof(cellPool));
        }

        protected override void OnAttach()
        {
            _model.State.Subscribe(OnHandleState).AddTo(View);
        }

        protected override void OnDetach()
        {
            base.OnDetach();
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
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }
        
        private void RebuildGrid()
        {
            Vector3 scale = new Vector3(_model.Scale, _model.Scale, 1);
            foreach (Cell cell in _model.Grid)
            {
                var view = _cellPool.Spawn();
                view.transform.SetParent(View.transform, false);
                
                view.transform.localPosition = cell.Position;
                view.transform.localScale = scale;
                view.SetBlock(cell.Type);
            }

            _model.ChangeState(FieldState.Ready);
        }
    }
}