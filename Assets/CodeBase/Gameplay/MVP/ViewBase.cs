using System;
using CodeBase.Shared.MVP;
using UnityEngine;

namespace CodeBase.Gameplay.MVP
{
    public class ViewBase : MonoBehaviour, IView
    {
        public bool IsVisible => gameObject.activeSelf;

        public event Action Shown;
        public event Action Hidden;
        
        public virtual void Show()
        {
            if (IsVisible) return;
            gameObject.SetActive(true);
            Shown?.Invoke();
        }

        public virtual void Hide()
        {
            if (!IsVisible) return;
            gameObject.SetActive(false);
            Hidden?.Invoke();
        }
    }
}