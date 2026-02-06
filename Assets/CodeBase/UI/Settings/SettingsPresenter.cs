using CodeBase.UI.MVP;
using CodeBase.UI.StateMachine;

namespace CodeBase.UI.Settings
{
    public sealed class SettingsPresenter : PresenterBase<SettingsView>
    {
        private readonly WindowStateMachine _windowStateMachine;

        public SettingsPresenter(SettingsView view, WindowStateMachine windowStateMachine) : base(view)
        {
            _windowStateMachine = windowStateMachine;
        }

        protected override void OnAttach()
        {
            View.CloseClicked += OnCloseClicked;
            Window.Opened += OnWindowOpened;
            Window.Closed += OnWindowClosed;
        }

        protected override void OnDetach()
        {
            View.CloseClicked -= OnCloseClicked;
            Window.Opened -= OnWindowOpened;
            Window.Closed -= OnWindowClosed;
        }

        private void OnCloseClicked()
        {
            // Go back to the previous window (likely Menu)
            _windowStateMachine.Back();
        }

        protected override void OnDispose()
        {
        }
        
        private void OnWindowOpened() => View.Show();
        private void OnWindowClosed() => View.Hide();
    }
}
