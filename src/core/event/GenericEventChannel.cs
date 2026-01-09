using Godot;
using System;

namespace GameSystems.Event;

public abstract partial class GenericEventChannel<T> : Resource
{
    private Action<T> _eventRaised;

    public event Action<T> EventRaised
    {
        add => Subscribe(value);
        remove => Unsubscribe(value);
    }

    public void Subscribe(Action<T> action)
    {
        if (EventBus.Instance == null)
        {
            _eventRaised += action;
        }
        else
        {
            EventBus.Instance.Subscribe(ResourcePath, action);
        }
    }

    public void Unsubscribe(Action<T> action)
    {
        if (EventBus.Instance == null)
        {
            _eventRaised -= action;
        }
        else
        {
            EventBus.Instance.Unsubscribe(ResourcePath, action);
        }
    }

    public void SendEvent(T parameter)
    {
        if (EventBus.Instance == null)
        {
            _eventRaised?.Invoke(parameter);
        }
        else
        {
            EventBus.Instance.SendEvent(ResourcePath, parameter);
        }
    }
}
