using System;
using CodeBase.Infrastructure.StateMachine;
using CodeBase.Infrastructure.StateMachine.States;
using CodeBase.UI.MVP;
using CodeBase.UI.StateMachine;
using UnityEngine;

namespace CodeBase.UI.Menu
{
    public sealed class MenuPresenter : PresenterBase<MenuView>
    {
        private readonly WindowStateMachine _windowStateMachine;
        private readonly AppStateMachine _appStateMachine;
        
        public MenuPresenter(
            MenuView view,
            WindowStateMachine windowStateMachine, 
            AppStateMachine appStateMachine) : base(view)
        {
            _windowStateMachine = windowStateMachine ?? throw new ArgumentNullException(nameof(windowStateMachine));
            _appStateMachine = appStateMachine ?? throw new ArgumentNullException(nameof(appStateMachine));
        }
        
        protected override void OnAttach()
        {
            base.OnAttach();
            View.PlayClicked += OnPlayClicked;
            View.SettingsClicked += OnSettingsClicked;
        }

        protected override void OnDetach()
        {
            base.OnDetach();
            View.PlayClicked -= OnPlayClicked;
            View.SettingsClicked -= OnSettingsClicked;
            
        }

        protected override void OnDispose()
        { }

        private void OnPlayClicked()
        {
            Debug.Log("Menu clicked");
            _appStateMachine.Enter<GameplayState>();
        }

        private void OnSettingsClicked()
        {
            _windowStateMachine.Open(WindowType.Settings);
        }
    }
}
