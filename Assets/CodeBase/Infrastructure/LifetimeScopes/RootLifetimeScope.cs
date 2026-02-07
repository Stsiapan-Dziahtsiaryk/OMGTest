using CodeBase.Gameplay.Controller;
using CodeBase.Gameplay.Field;
using CodeBase.Gameplay.Field.Config;
using CodeBase.Gameplay.Level;
using CodeBase.Infrastructure.Scenes;
using CodeBase.Infrastructure.StateMachine;
using CodeBase.Infrastructure.StateMachine.States;
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
            
            // App state machine core
            builder.Register<AppStateMachine>(Lifetime.Singleton);

            // States
            builder.Register<BootstrapState>(Lifetime.Singleton);
            builder.Register<EnterState>(Lifetime.Singleton);
            builder.Register<GameplayState>(Lifetime.Singleton);
        }
    }
}