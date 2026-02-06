using System;
using CodeBase.UI.Menu;
using CodeBase.UI.Settings;
using CodeBase.UI.StateMachine;
using VContainer;
using VContainer.Unity;

namespace CodeBase.Infrastructure.Composition
{
    public sealed class EnterCompositionRoot : IInitializable, IDisposable
    {
        private readonly IObjectResolver _resolver;

        public EnterCompositionRoot(IObjectResolver resolver)
        {
            _resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
        }

        public void Initialize()
        {
            _resolver
                .Resolve<WindowStateMachine>()
                .RegisterWindow(WindowType.Menu, _resolver.Resolve<MenuPresenter>().Window);
            _resolver.Resolve<WindowStateMachine>().RegisterWindow(WindowType.Settings, _resolver.Resolve<SettingsPresenter>().Window);
            
            _resolver.Resolve<MenuPresenter>().Attach();
            _resolver.Resolve<SettingsPresenter>().Attach();
           
        }

        public void Dispose()
        {
            _resolver.Resolve<MenuPresenter>().Dispose();
            _resolver.Resolve<SettingsPresenter>().Dispose();
        }
    }
}
