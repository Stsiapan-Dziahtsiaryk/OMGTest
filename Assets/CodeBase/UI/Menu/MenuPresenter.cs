using System;
using CodeBase.Gameplay.Controller;
using CodeBase.UI.MVP;
using CodeBase.UI.StateMachine;

namespace CodeBase.UI.Menu
{
    public sealed class MenuPresenter : PresenterBase<MenuView>
    {
        private readonly GameStateMachine _gameMachine;
        private readonly WindowStateMachine _windowStateMachine;
        
        public MenuPresenter(
            MenuView view,
            GameStateMachine gameMachine,
            WindowStateMachine windowStateMachine) : base(view)
        {
            _gameMachine = gameMachine ?? throw new ArgumentNullException(nameof(gameMachine));
            _windowStateMachine = windowStateMachine ?? throw new ArgumentNullException(nameof(windowStateMachine));
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
            _windowStateMachine.Open(WindowType.HUD);            
        }
    }
}
