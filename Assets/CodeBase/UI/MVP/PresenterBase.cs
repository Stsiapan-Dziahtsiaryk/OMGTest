using System;
using CodeBase.Shared.MVP;
using CodeBase.UI.StateMachine;

namespace CodeBase.UI.MVP
{
    public abstract class PresenterBase<TView> 
        : IPresenter<TView> 
        where TView : ViewBase, IView
    {

        protected PresenterBase(TView view)
        {
            View = view;
            Window = new Window();
        }

        public TView View { get; }
        public Window Window { get; private set; }
        public bool IsAttached { get; private set; }
        
        public void Attach()
        {
            if (IsAttached) return;
            IsAttached = true;
            OnAttach();
        }

        public void Detach()
        {
            if (!IsAttached) return;
            IsAttached = false;
            OnDetach();
        }

        protected virtual void OnAttach() { }
        protected virtual void OnDetach() { }

        public virtual void Dispose()
        {
            if (IsAttached)
                Detach();
            OnDispose();
        }

        protected abstract void OnDispose();
        
        protected virtual void HandleShow() => View.Show();
        protected virtual void HandleHide() => View.Hide();
    }
}
