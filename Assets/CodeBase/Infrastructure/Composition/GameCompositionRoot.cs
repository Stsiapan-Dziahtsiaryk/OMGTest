using System;
using CodeBase.Domain.Database;
using CodeBase.Domain.Database.Data;
using CodeBase.Domain.Database.Sheets;
using CodeBase.Gameplay.Controller;
using CodeBase.Gameplay.Environment;
using CodeBase.Gameplay.Environment.View;
using CodeBase.Gameplay.Field;
using CodeBase.UI.HUD;
using CodeBase.UI.StateMachine;
using UnityEngine;
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
            
            Application.focusChanged += SaveSession;
#if UNITY_EDITOR
            Application.quitting += () =>
            {
                SaveSession(false);
            }; 
#endif
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
            Application.focusChanged -= SaveSession;
        }

        private void SaveSession(bool hasFocus)
        {
            var data = _resolver.Resolve<FieldModel>().GetData();
            _resolver
                .Resolve<DatabaseService>()
                .GetSheet<LevelSheet>()
                .Save(new LevelSnapshot(!hasFocus, data));
        }
    }
}