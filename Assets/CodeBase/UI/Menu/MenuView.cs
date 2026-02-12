using System;
using CodeBase.UI.MVP;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Menu
{
    public sealed class MenuView : ViewBase
    {
        [SerializeField] private Button _menuButton;

        public event Action PlayClicked;

        private void OnEnable()
        {
            if (_menuButton != null)
                _menuButton.onClick.AddListener(OnMenuButton);
        }

        private void OnDisable()
        {
            if (_menuButton != null)
                _menuButton.onClick.RemoveListener(OnMenuButton);
        }

        private void OnMenuButton() => PlayClicked?.Invoke();
    }
}
