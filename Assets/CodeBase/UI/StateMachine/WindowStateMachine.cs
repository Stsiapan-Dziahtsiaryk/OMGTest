using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.UI.StateMachine
{
    public sealed class WindowStateMachine
    {
        private readonly Dictionary<WindowType, Window> _windows = new Dictionary<WindowType, Window>();
        private readonly Stack<WindowType> _history = new Stack<WindowType>();
        
        public WindowType Current { get; private set; }

        public void RegisterWindow(WindowType key, Window window)
        {
            if (!_windows.TryAdd(key, window))
                Debug.LogWarning($"Window {key} already registered!");
        }

        public void Open(WindowType key, bool pushToHistory = true)
        {
            if(_windows.TryGetValue(key, out var next) == false) return;
            if (pushToHistory && Current != WindowType.Invalid)
                _history.Push(Current);
            Close(Current);
            Current = key;
            next?.Open();
        }

        public void OpenAsStack(WindowType key, bool pushToHistory = true)
        {
            if(_windows.TryGetValue(key, out var window) == false) return;
            if (pushToHistory && Current != WindowType.Invalid)
                _history.Push(Current);
            Debug.Log($"OpenAsStack: {key}");
            window?.Open();
        }
        
        public void Back()
        {
            if (_history.Count == 0) return;
            var prevType = _history.Pop();
            Open(prevType, pushToHistory: false);
        }

        public void CloseAll()
        {
            Close(Current);
            Current = WindowType.Invalid;
            _history.Clear();
        }
        
        public void Close(WindowType key)
        {
            if (key == WindowType.Invalid) return;
            if(_windows.TryGetValue(key, out Window window) == false) return;
            window.Close();
        }
        
        public void Clean()
        {
            _windows.Clear();
            _history.Clear();
        }
        
        public void CleanHistory()
        {
            if(_history.Count == 0) return;
            var lastActive = _history.Peek();
            _history.Clear();
            _history.Push(lastActive);
        }
    }
}