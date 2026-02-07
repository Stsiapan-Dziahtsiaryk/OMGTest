using System;
using CodeBase.Shared.MVP;
using UnityEngine;

namespace CodeBase.UI.MVP
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class ViewBase : MonoBehaviour, IView
    {
        [SerializeField] protected CanvasGroup _canvasGroup;
        
        public bool IsVisible => gameObject.activeSelf;

        public event Action Shown;
        public event Action Hidden;

        private void OnValidate()
        {
            if(!_canvasGroup) 
                _canvasGroup = GetComponent<CanvasGroup>();
        }

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
