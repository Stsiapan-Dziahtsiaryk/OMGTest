using System;
using CodeBase.Gameplay.Controller;
using CodeBase.Gameplay.Environment;
using CodeBase.Gameplay.Environment.View;
using CodeBase.Gameplay.Field;
using CodeBase.UI.HUD;
using CodeBase.UI.StateMachine;
using VContainer;
using VContainer.Unity;

namespace CodeBase.Infrastructure.Composition
{
    public class GameCompositionRoot : 
        IInitializable,
        IStartable,
        IDisposable
    {
        private readonly IObjectResolver _resolver;

        public GameCompositionRoot(IObjectResolver resolver)
        {
            _resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
        }

        public void Initialize()
        {
            _resolver.Resolve<WindowStateMachine>().Clean();
            
            _resolver.Resolve<CellView.Pool>().Start();
            _resolver.Resolve<BalloonView.Pool>().Start();
            
            // Register UI Window
            _resolver
                .Resolve<WindowStateMachine>()
                .RegisterWindow(WindowType.HUD, _resolver.Resolve<HUDPresenter>().Window);
            
            
            // Attach presenters            
            _resolver.Resolve<FieldPresenter>().Attach();
            _resolver.Resolve<HUDPresenter>().Attach();
            _resolver.Resolve<BalloonSpawnerPresenter>().Attach();
        }

        public void Start()
        {
            _resolver
                .Resolve<GameStateMachine>()
                .HandleNewGame();
        }

        public void Dispose()
        {
            _resolver.Resolve<FieldPresenter>().Dispose();
            _resolver.Resolve<HUDPresenter>().Dispose();
            _resolver.Resolve<BalloonSpawnerPresenter>().Detach();
        }
    }
}