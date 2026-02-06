using System;

namespace CodeBase.Shared.MVP
{
    public interface IView
    {
        bool IsVisible { get; }

        event Action Shown;
        event Action Hidden;

        void Show();
        void Hide();
    }
}
