using Godot;
using System;

namespace GameSystems.Event
{
    public abstract partial class GenericEventChannel<T> : Resource
    {
        public Action<T> EventRaised;

        public void RaiseEvent(T parameter)
        {
            EventRaised?.Invoke(parameter);
        }
    }
}