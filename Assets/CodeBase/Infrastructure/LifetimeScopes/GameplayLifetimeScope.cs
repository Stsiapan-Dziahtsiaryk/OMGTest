using CodeBase.Gameplay.Controller;
using CodeBase.Gameplay.Environment;
using CodeBase.Gameplay.Environment.View;
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
        [SerializeField] private BalloonView _balloonPrefab;
        [SerializeField] private BalloonSpawnerSettings _balloonSpawnerSettings;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(_balloonSpawnerSettings);
            
            // Pools
            builder
                .RegisterPool<CellView, CellView.Pool>(
                    _cellPrefab,
                    "Cell's Pool",
                    60);
            builder
                .RegisterPool<BalloonView, BalloonView.Pool>(
                    _balloonPrefab,
                    "Balloon's Pool",
                    9);

            
            // Views
            builder.RegisterComponentInHierarchy<FieldView>();
            builder.RegisterComponentInHierarchy<HUDView>();
            builder.RegisterComponentInHierarchy<BalloonSpawnerView>();
            
            // Presenters
            builder.Register<FieldPresenter>(Lifetime.Singleton);
            builder.Register<HUDPresenter>(Lifetime.Singleton);
            builder.Register<BalloonSpawnerPresenter>(Lifetime.Singleton);

            builder.RegisterEntryPoint<GameCompositionRoot>();
            builder.RegisterEntryPoint<InputService>().AsSelf();
            builder.RegisterEntryPoint<BalloonSpawner>().AsSelf();
            
        }
    }
}