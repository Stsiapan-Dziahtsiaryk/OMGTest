using System;
using CodeBase.Infrastructure.Extensions;
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
        }
        
        [SerializeField] private BlockView _blockView;
        
        public Vector2Int ID { get; private set; }

        public Action<Vector2Int> Selecting;
        
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
            Debug.Log($"Cell {ID} state changed to {data.State}");
            switch (data.State)
            {
                case Cell.State.Invalid:
                    break;
                case Cell.State.Move:
                    _blockView.SetBlock(data.Type);
                    break;
                case Cell.State.Destroy:
                    
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}