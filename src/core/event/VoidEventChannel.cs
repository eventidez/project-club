using Godot;
using System;

namespace GameSystems.Event;

[GlobalClass]
public partial class VoidEventChannel : Resource
{
    public string EventId => GetRid().ToString();

    [Export(PropertyHint.MultilineText, "20")] public string EventName { get; set; }

    private Action _eventRaised;

    public event Action EventRaised
    {
        add => Subscribe(value);
        remove => Unsubscribe(value);
    }

    public void Subscribe(Action action)
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

    public void Unsubscribe(Action action)
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


    public void SendEvent()
    {
        if (EventBus.Instance == null)
        {
            _eventRaised?.Invoke();
        }
        else
        {
            EventBus.Instance.SendEvent(EventId);
        }
    }
}
