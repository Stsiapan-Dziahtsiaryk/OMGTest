using UnityEngine;
using VContainer;

namespace CodeBase.Infrastructure.Extensions
{
    public static class PoolRegister
    {
        public static RegistrationBuilder RegisterPool<TE, T>(
            this IContainerBuilder resolver,
            TE prefab,
            string parentName = "Parent",
            int amount = 10)
            where T : MonoPool<TE>
            where TE : MonoBehaviour
        {
            return resolver.Register<T>(Lifetime.Singleton)
                .WithParameter("prefab", prefab)
                .WithParameter("maxInstances", amount)
                .WithParameter("parentName", parentName);
        }
    }
}