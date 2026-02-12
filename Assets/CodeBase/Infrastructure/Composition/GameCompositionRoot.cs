using System;
using CodeBase.Domain.Database;
using CodeBase.Domain.Database.Data;
using CodeBase.Domain.Database.Sheets;
using CodeBase.Gameplay.Controller;
using CodeBase.Gameplay.Environment;
using CodeBase.Gameplay.Environment.View;
using CodeBase.Gameplay.Field;
using CodeBase.Gameplay.Field.Config;
using CodeBase.UI.Curtain;
using CodeBase.UI.HUD;
using CodeBase.UI.Menu;
using CodeBase.UI.StateMachine;
using R3;
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
        
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        
        public GameCompositionRoot(IObjectResolver resolver)
        {
            _resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
        }

        public void Initialize()
        {
            Application.focusChanged += SaveSession;
#if UNITY_EDITOR
            Application.quitting += () =>
            {
                SaveSession(false);
            }; 
#endif
            
            _resolver.Resolve<WindowStateMachine>().Clean();
            
            _resolver.Resolve<CellView.Pool>().Start();
            _resolver.Resolve<BalloonView.Pool>().Start();
            
            // Register UI Window
            _resolver
                .Resolve<WindowStateMachine>()
                .RegisterWindow(WindowType.Menu, _resolver.Resolve<MenuPresenter>().Window);
            
            _resolver
                .Resolve<WindowStateMachine>()
                .RegisterWindow(WindowType.HUD, _resolver.Resolve<HUDPresenter>().Window);
            
            _resolver
                .Resolve<WindowStateMachine>()
                .RegisterWindow(WindowType.Curtain, _resolver.Resolve<CurtainPresenter>().Window);
            
            
            // Attach presenters            
            _resolver.Resolve<MenuPresenter>().Attach();
            _resolver.Resolve<FieldPresenter>().Attach();
            _resolver.Resolve<HUDPresenter>().Attach();
            _resolver.Resolve<BalloonSpawnerPresenter>().Attach();
            _resolver.Resolve<CurtainPresenter>().Attach();

            _resolver
                .Resolve<FieldModel>()
                .State
                .Subscribe(OnNextLevel)
                .AddTo(_disposables);
        }

        public void Start()
        {
            _resolver.Resolve<WindowStateMachine>().Open(WindowType.Menu);
        }

        public void Dispose()
        {
            _resolver.Resolve<MenuPresenter>().Dispose();
            _resolver.Resolve<FieldPresenter>().Dispose();
            _resolver.Resolve<HUDPresenter>().Dispose();
            _resolver.Resolve<BalloonSpawnerPresenter>().Detach();
            _resolver.Resolve<CurtainPresenter>().Detach();

            _disposables.Dispose();
            
            Application.focusChanged -= SaveSession;
        }

        private void SaveSession(bool hasFocus)
        {
            var data = _resolver.Resolve<FieldModel>().GetData();
            if(data.Grid == null || data.Grid.Length == 0) return;
            _resolver
                .Resolve<DatabaseService>()
                .GetSheet<LevelSheet>()
                .Save(new LevelSnapshot(!hasFocus, data));
        }

        private void OnNextLevel(FieldState state)
        {
            if(state == FieldState.Finish)
            {
                _resolver
                    .Resolve<WindowStateMachine>()
                    .OpenAsStack(WindowType.Curtain, false);
                _resolver
                    .Resolve<GameStateMachine>()
                    .NextLevel();
            }
        }
    }
}