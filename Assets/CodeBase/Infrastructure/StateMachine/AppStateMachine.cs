using System;
using System.Collections.Generic;
using CodeBase.Infrastructure.StateMachine.States;
using UnityEngine;

namespace CodeBase.Infrastructure.StateMachine
{
    public sealed class AppStateMachine
    {
        private readonly Dictionary<Type, IExitState> _states = new();
        
        public IExitState Current { get; private set; }

        public void Register<T>(T state) where T : IExitState
            => _states[typeof(T)] = state;
        
        public void Enter<T>() where T : class, IState
        {
            if (!_states.TryGetValue(typeof(T), out var next))
            {
                Debug.LogWarning($"State {typeof(T).Name} is not registered");
                return;
            }
            Current?.Exit();
            Current = next;
            ((IState)next)?.Enter();
        }

        public void Enter<T, TPayload>(TPayload payload) where T : class, IPayloadedState<TPayload>
        {
            if (!_states.TryGetValue(typeof(T), out var boxed))
            {
                Debug.LogWarning($"State {typeof(T).Name} is not registered");
                return;
            }
            Current?.Exit();
            Current = boxed;
            ((IPayloadedState<TPayload>)boxed)?.Enter(payload);
        }
        
        public void Exit() // ???
        {
            Current?.Exit();
        }
    }
}