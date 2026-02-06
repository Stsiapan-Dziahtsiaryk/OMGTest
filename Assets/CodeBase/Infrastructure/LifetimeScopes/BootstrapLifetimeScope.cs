using CodeBase.Infrastructure.StateMachine;
using VContainer;
using VContainer.Unity;

namespace CodeBase.Infrastructure.LifetimeScopes
{
    public class BootstrapLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            // Entry point to start the state machine
            builder.RegisterEntryPoint<AppStartup>();
        }
    }
}