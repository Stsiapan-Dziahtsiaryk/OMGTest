using System;
using CodeBase.Gameplay.Controller;
using CodeBase.Gameplay.Environment;
using CodeBase.UI.MVP;
using CodeBase.UI.StateMachine;

namespace CodeBase.UI.Menu
{
    public sealed class MenuPresenter : PresenterBase<MenuView>
    {
        private readonly GameStateMachine _gameMachine;
        private readonly WindowStateMachine _windowStateMachine;
        private readonly BalloonSpawner _balloonSpawner;
        
        public MenuPresenter(
            MenuView view,
            GameStateMachine gameMachine,
            WindowStateMachine windowStateMachine, 
            BalloonSpawner balloonSpawner) : base(view)
        {
            _gameMachine = gameMachine ?? throw new ArgumentNullException(nameof(gameMachine));
            _windowStateMachine = windowStateMachine ?? throw new ArgumentNullException(nameof(windowStateMachine));
            _balloonSpawner = balloonSpawner ?? throw new ArgumentNullException(nameof(balloonSpawner));
        }
        
        protected override void OnAttach()
        {
            base.OnAttach();
            View.PlayClicked += OnPlayClicked;
        }

        protected override void OnDetach()
        {
            base.OnDetach();
            View.PlayClicked -= OnPlayClicked;
        }

        protected override void OnDispose()
        { }

        private void OnPlayClicked()
        {
            _gameMachine.HandleNewGame();
            _balloonSpawner.Start();
            _windowStateMachine.Open(WindowType.HUD);            
            _windowStateMachine.OpenAsStack(WindowType.Curtain, false);
        }
    }
}
