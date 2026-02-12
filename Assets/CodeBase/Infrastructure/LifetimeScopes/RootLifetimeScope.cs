using CodeBase.Domain.Configs;
using CodeBase.Domain.Database;
using CodeBase.Gameplay.Controller;
using CodeBase.Gameplay.Field;
using CodeBase.Gameplay.Field.Config;
using CodeBase.Gameplay.Level;
using CodeBase.Infrastructure.AppRunner;
using CodeBase.Infrastructure.Scenes;
using CodeBase.UI.StateMachine;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace CodeBase.Infrastructure.LifetimeScopes
{
    public class RootLifetimeScope : LifetimeScope
    {
        [SerializeField] private GameSettings _settings;
        [SerializeField] private LevelConfig _config;
        
        protected override void Configure(IContainerBuilder builder)
        {
            // configs & settings
            builder.RegisterInstance(_config);
            builder.RegisterInstance(_settings);
            
            // Services
            builder.Register<SceneService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<WindowStateMachine>(Lifetime.Singleton);
            builder.Register<GameStateMachine>(Lifetime.Singleton);
            
            // Game services & models 
            builder.Register<FieldModel>(Lifetime.Singleton);
            builder.Register<LevelBuilder>(Lifetime.Singleton);
            
            // Entry point to start the state machine
            builder.RegisterEntryPoint<AppStartup>();
            
            builder.RegisterEntryPoint<DatabaseService>().AsSelf();
            builder.RegisterEntryPoint<LevelConfigLoader>().AsSelf();
            
        }
    }
}