using CodeBase.Infrastructure.Extensions;
using UnityEngine;
using VContainer.Unity;

namespace CodeBase.Gameplay.Field
{
    public class BlockView : MonoBehaviour
    {
        public class Pool : MonoPool<BlockView>
        {
            public Pool(
                LifetimeScope container,
                BlockView prefab,
                int maxInstances = 10,
                string parentName = "Parent of Pool")
                : base(container, prefab, maxInstances, parentName)
            {
            }
        }
        
        
        
    }
}