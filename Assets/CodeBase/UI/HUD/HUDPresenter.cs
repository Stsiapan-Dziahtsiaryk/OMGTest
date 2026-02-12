using System;
using CodeBase.Gameplay.Controller;
using CodeBase.UI.MVP;
using CodeBase.UI.StateMachine;
using UnityEngine;

namespace CodeBase.UI.HUD
{
    public class HUDPresenter : PresenterBase<HUDView>
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly WindowStateMachine _windowStateMachine;
        
        public HUDPresenter(
            HUDView view,
            GameStateMachine gameStateMachine, 
            WindowStateMachine windowStateMachine) : base(view)
        {
            _gameStateMachine = gameStateMachine ?? throw new ArgumentNullException(nameof(gameStateMachine));
            _windowStateMachine = windowStateMachine ?? throw new ArgumentNullException(nameof(windowStateMachine));
        }

        protected override void OnDispose()
        {
            
        }

        protected override void OnAttach()
        {
            base.OnAttach();
            View.RestartClick += OnRestart;
            View.NextClick += OnNext;
        }

        protected override void OnDetach()
        {
            base.OnDetach();
            View.RestartClick -= OnRestart;
            View.NextClick -= OnNext;
        }
        
        private void OnRestart()
        {
            Debug.Log("Restart");
            _windowStateMachine.OpenAsStack(WindowType.Curtain, false);
            _gameStateMachine.RestartLevel();
            
        }
        
        private void OnNext()
        {
            Debug.Log("Next");
            _windowStateMachine.OpenAsStack(WindowType.Curtain, false);
            _gameStateMachine.NextLevel();
        }
    }
}