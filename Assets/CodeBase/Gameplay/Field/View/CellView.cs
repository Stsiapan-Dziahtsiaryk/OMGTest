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

        private void OnEnable()
        {
            _blockView.Destroyed += HandleFinishAction;
        }

        private void OnDisable()
        {
            _blockView.Destroyed -= HandleFinishAction;
            _tween?.Kill();
        }

        public void Initialize(Vector2Int id, AnimationClip[] clips)
        {
            ID = id;
            _blockView.Initialize(id.x * (id.y + 1), clips);
        }
        
        public void Initialize(Vector2Int id, int type)
        {
            ID = id;
            _blockView.SetLayer(id.x * (id.y + 1));
            _blockView.SetBlock(type);
        }

        public void Selected()
        {
            Selecting?.Invoke(ID);
        }

        public void HandleState(CellDto data)
        {
            switch (data.State)
            {
                case Cell.State.Invalid:
                    break;
                case Cell.State.Move:
                    Moving(data);
                    break;
                case Cell.State.Destroy:
                    Debug.Log($"Destroy {ID.x}");
                    _blockView.DestroyBlock(HandleFinishAction);
                    break;
                case Cell.State.Idle:
                    Debug.Log($"Idle {ID.x}");
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
                        Callback?.Invoke(data.GridPosition);
                    });
        }
    }
}