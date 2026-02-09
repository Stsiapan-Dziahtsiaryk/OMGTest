using CodeBase.Gameplay.Controller;
using CodeBase.Gameplay.Field;
using CodeBase.Infrastructure.Composition;
using CodeBase.Infrastructure.Extensions;
using CodeBase.UI.HUD;
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
            builder.RegisterComponentInHierarchy<HUDView>();
            
            // Presenters
            builder.Register<FieldPresenter>(Lifetime.Singleton);
            builder.Register<HUDPresenter>(Lifetime.Singleton);

            builder.RegisterEntryPoint<GameCompositionRoot>();
            builder.RegisterEntryPoint<InputService>().AsSelf();
        }
    }
}