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

        public void SetBlock(int type)
        {
            _blockView.SetBlock(type);
        }
    }
}