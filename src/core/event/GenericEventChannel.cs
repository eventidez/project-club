using Godot;
using System;

namespace GameSystems.Event
{
    public abstract partial class GenericEventChannel<T> : Resource
    {
        public Action<T> OnEventRaised;

        public void RaiseEvent(T parameter)
        {
            OnEventRaised?.Invoke(parameter);
        }
    }
}