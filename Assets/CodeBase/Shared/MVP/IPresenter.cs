using System;

namespace CodeBase.Shared.MVP
{
    public interface IPresenter<out TView> : IDisposable
        where TView : IView
    {
        TView View { get; }

        void Attach();
        void Detach();
    }
}
