using System;
using CodeBase.Infrastructure.Extensions;
using DG.Tweening;
using UnityEngine;
using VContainer.Unity;

namespace CodeBase.Gameplay.Field
{
    public class CellView : MonoBehaviour
    {
        public class Pool : MonoPool<CellView>
        {
            public Pool(
                LifetimeScope container,
                CellView prefab,
                int maxInstances = 10,
                string parentName = "Parent of Pool")
                : base(container, prefab, maxInstances, parentName)
            {
            }

            protected override void DespawnAll(CellView instance)
            {
                instance.Callback = null;
                instance.Selecting = null;
                base.DespawnAll(instance);
            }
        }

        [SerializeField] private BlockView _blockView;
        private Tween _tween;

        public Vector2Int ID { get; private set; }

        public event Action<Vector2Int> Selecting;
        public event Action<Vector2Int> Callback;

        private void OnDisable()
        {
            _tween?.Kill();
        }

        public void SetBlock(Vector2Int id, int type)
        {
            ID = id;
            _blockView.SetLayer(id.x * (id.y + 1));
            _blockView.SetBlock(type);
        }

        public void Selected()
        {
            Debug.Log($"Selected {ID}");
            Selecting?.Invoke(ID);
        }

        public void HandleState(CellDto data)
        {
            // ID = data.GridPosition;
            switch (data.State)
            {
                case Cell.State.Invalid:
                    break;
                case Cell.State.Move:
                    Debug.Log($"Moving {ID} to {data.Position}");
                    Moving(data);
                    break;
                case Cell.State.Destroy:
                    _blockView.DestroyBlock(HandleFinishAction);
                    // HandleFinishAction();
                    break;
                case Cell.State.Idle:
                    // _blockView.SetBlock(data.Type);
                    Debug.Log($"Idle {ID}");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void HandleFinishAction()
        {
            Callback?.Invoke(ID);
        }

        private void Moving(CellDto data)
        {
            _tween?.Kill();
            _blockView.SetLayer(data.GridPosition.x * (data.GridPosition.y + 1));

            _tween =
                transform
                    .DOLocalMove(data.Position, 0.25f)
                    .OnKill(() =>
                    {
                        ID = data.GridPosition;
                        Debug.Log($"Moving finished {ID}");
                        Callback?.Invoke(data.GridPosition);
                    });
        }
    }
}