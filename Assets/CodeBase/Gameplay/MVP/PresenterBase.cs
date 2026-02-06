using CodeBase.Shared.MVP;

namespace CodeBase.Gameplay.MVP
{
    public abstract class PresenterBase<TView> 
        : IPresenter<TView>
    where TView: ViewBase, IView
    {
        public TView View { get; }

        public bool IsAttached { get; private set; }

        protected PresenterBase(TView view)
        {
            View = view;
        }

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

        protected virtual void OnDispose() { }
    }
}