using System;
using CodeBase.UI.MVP;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.HUD
{
    public class HUDView : ViewBase
    {
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _nextButton;

        public event Action RestartClick;
        public event Action NextClick;

        private void OnEnable()
        {
            if(_restartButton != null) 
                _restartButton.onClick.AddListener(HandleRestartClick);
            if(_nextButton != null) 
                _nextButton.onClick.AddListener(HandleNextClick);
        }

        private void OnDisable()
        {
            if(_restartButton != null)
                _restartButton.onClick.RemoveListener(HandleRestartClick);
            if(_nextButton != null)
                _nextButton.onClick.RemoveListener(HandleNextClick);
        }
        
        private void HandleRestartClick() => RestartClick?.Invoke();
        private void HandleNextClick() => NextClick?.Invoke();
    }
}