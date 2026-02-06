using CodeBase.UI.MVP;
using CodeBase.UI.StateMachine;
using UnityEngine;

namespace CodeBase.UI.Menu
{
    public sealed class MenuPresenter : PresenterBase<MenuView>
    {
        private readonly WindowStateMachine _windowStateMachine;
        public MenuPresenter(
            MenuView view,
            WindowStateMachine windowStateMachine) : base(view)
        {
            _windowStateMachine = windowStateMachine;
        }
        
        protected override void OnAttach()
        {
            View.MenuClicked += OnMenuClicked;
            View.SettingsClicked += OnSettingsClicked;
            
            Window.Opened += OnWindowOpened;
            Window.Closed += OnWindowClosed;
        }

        protected override void OnDetach()
        {
            View.MenuClicked -= OnMenuClicked;
            View.SettingsClicked -= OnSettingsClicked;
            
            Window.Opened -= OnWindowOpened;
            Window.Closed -= OnWindowClosed;
        }

        protected override void OnDispose()
        { }

        private void OnMenuClicked()
        {
            Debug.Log("Menu clicked");
        }

        private void OnSettingsClicked()
        {
            _windowStateMachine.Open(WindowType.Settings);
        }

        private void OnWindowOpened() => View.Show();
        private void OnWindowClosed() => View.Hide();
    }
}
