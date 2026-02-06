using CodeBase.Infrastructure.Scenes;
using CodeBase.Infrastructure.StateMachine;
using CodeBase.Infrastructure.StateMachine.States;
using CodeBase.UI.StateMachine;
using VContainer;
using VContainer.Unity;

namespace CodeBase.Infrastructure.LifetimeScopes
{
    public class RootLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            // Services
            builder.Register<SceneService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<WindowStateMachine>(Lifetime.Singleton);
            
            // App state machine core
            builder.Register<AppStateMachine>(Lifetime.Singleton);

            // States
            builder.Register<BootstrapState>(Lifetime.Singleton);
            builder.Register<EnterState>(Lifetime.Singleton);
            builder.Register<GameplayState>(Lifetime.Singleton);
       
        }
    }
}