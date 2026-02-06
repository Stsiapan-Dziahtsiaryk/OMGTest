using System;

namespace CodeBase.UI.StateMachine
{
    public class Window
    {
        public bool IsOpen { get; private set; }
        
        public event Action Opened;
        public event Action Closed;

        public void Open()
        {
            if (IsOpen) return;
            IsOpen = true;
            Opened?.Invoke();
        }

        public void Close()
        {
            if (!IsOpen) return;
            IsOpen = false;
            Closed?.Invoke();
        }
    }
}