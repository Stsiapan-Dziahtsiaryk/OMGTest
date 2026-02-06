using System;
using CodeBase.UI.MVP;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Settings
{
    // Simple Settings window view following MVP base View
    public sealed class SettingsView : ViewBase
    {
        [SerializeField] private Button _closeButton;

        public event Action CloseClicked;

        private void OnEnable()
        {
            if (_closeButton != null)
                _closeButton.onClick.AddListener(OnCloseButton);
        }

        private void OnDisable()
        {
            if (_closeButton != null)
                _closeButton.onClick.RemoveListener(OnCloseButton);
        }

        private void OnCloseButton() => CloseClicked?.Invoke();

        // Optional setter to wire button at runtime if needed
        public void SetCloseButton(Button button)
        {
            if (_closeButton != null)
                _closeButton.onClick.RemoveListener(OnCloseButton);
            _closeButton = button;
            if (isActiveAndEnabled && _closeButton != null)
                _closeButton.onClick.AddListener(OnCloseButton);
        }
    }
}
