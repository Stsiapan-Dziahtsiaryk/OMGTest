using System;
using CodeBase.UI.MVP;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Menu
{
    public sealed class MenuView : ViewBase
    {
        [SerializeField] private Button _menuButton;
        [SerializeField] private Button _settingsButton;

        public event Action MenuClicked;
        public event Action SettingsClicked;

        private void OnEnable()
        {
            if (_menuButton != null)
                _menuButton.onClick.AddListener(OnMenuButton);
            if (_settingsButton != null)
                _settingsButton.onClick.AddListener(OnSettingsButton);
        }

        private void OnDisable()
        {
            if (_menuButton != null)
                _menuButton.onClick.RemoveListener(OnMenuButton);
            if (_settingsButton != null)
                _settingsButton.onClick.RemoveListener(OnSettingsButton);
        }

        private void OnMenuButton() => MenuClicked?.Invoke();
        private void OnSettingsButton() => SettingsClicked?.Invoke();
    }
}
