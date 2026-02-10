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
                    _blockView.DestroyBlock(HandleFinishAction);
                    // HandleFinishAction();
                    break;
                case Cell.State.Idle:
                    _blockView.SetBlock(data.Type);
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
            
            if (data.Type == -1)
            {
                _blockView.SetBlock(data.Type, HandleFinishAction);
            }
            else
            {
                Vector3 defaultPos = _blockView.transform.localPosition; 
                var target = (Vector3)data.Position - transform.position;
                _blockView.transform.localPosition += target; 
                _blockView.SetBlock(data.Type);
                
                _tween =
                    _blockView 
                        .transform
                        .DOLocalMove(defaultPos, 0.25f)
                        .OnKill(() =>
                        {
                            HandleFinishAction();
                        });    
            }
        }
    }
}