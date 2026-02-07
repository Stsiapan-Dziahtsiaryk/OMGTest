using System;
using CodeBase.Gameplay.Controller;
using CodeBase.UI.MVP;
using UnityEngine;

namespace CodeBase.UI.HUD
{
    public class HUDPresenter : PresenterBase<HUDView>
    {
        private readonly GameStateMachine _gameStateMachine;
        
        public HUDPresenter(
            HUDView view,
            GameStateMachine gameStateMachine) : base(view)
        {
            _gameStateMachine = gameStateMachine ?? throw new ArgumentNullException(nameof(gameStateMachine));
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
            _gameStateMachine.RestartLevel();
        }
        
        private void OnNext()
        {
            Debug.Log("Next");
            _gameStateMachine.NextLevel();
        }
    }
}