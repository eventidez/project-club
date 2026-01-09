using Godot;
using System;

namespace GameSystems.Event;

public abstract partial class GenericEventChannel<T> : Resource
{
    public string EventId => GetRid().ToString();

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
            EventBus.Instance.Subscribe(EventId, action);
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
            EventBus.Instance.Unsubscribe(EventId, action);
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
            EventBus.Instance.SendEvent(EventId, parameter);
        }
    }
}
