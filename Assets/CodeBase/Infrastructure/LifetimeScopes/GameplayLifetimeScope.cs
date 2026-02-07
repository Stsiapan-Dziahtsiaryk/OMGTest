using CodeBase.Gameplay.Field;
using CodeBase.Gameplay.Field.Config;
using CodeBase.Infrastructure.Composition;
using CodeBase.Infrastructure.Extensions;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace CodeBase.Infrastructure.LifetimeScopes
{
    public class GameplayLifetimeScope : LifetimeScope
    {
        [SerializeField] private CellView _cellPrefab;
        
        protected override void Configure(IContainerBuilder builder)
        {
            // Pools
            builder
                .RegisterPool<CellView, CellView.Pool>(_cellPrefab, "Cell's Pool", 60);
            
            // Views
            builder.RegisterComponentInHierarchy<FieldView>();
            
            // Presenters
            builder.Register<FieldPresenter>(Lifetime.Singleton);

            builder.RegisterEntryPoint<GameCompositionRoot>();
        }
    }
}