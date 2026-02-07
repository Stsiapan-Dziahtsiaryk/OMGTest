using CodeBase.Infrastructure.Composition;
using CodeBase.UI.Menu;
using CodeBase.UI.Settings;
using VContainer;
using VContainer.Unity;

namespace CodeBase.Infrastructure.LifetimeScopes
{
    public class EnterLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            // Views
            builder.RegisterComponentInHierarchy<MenuView>();
            builder.RegisterComponentInHierarchy<SettingsView>();
            
            // Presenters
            builder.Register<MenuPresenter>(Lifetime.Singleton);
            builder.Register<SettingsPresenter>(Lifetime.Singleton);
            
            builder.RegisterEntryPoint<EnterCompositionRoot>();
        }
    }
}