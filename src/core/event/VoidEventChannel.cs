using Godot;
using System;

namespace GameSystems.Event
{
    [GlobalClass]
    public partial class VoidEventChannel : Resource
    {
        [Export(PropertyHint.MultilineText, "20")] public string EventName { get; set; }

        private Action _eventRaised;

        public event Action EventRaised
        {
            add
            {
                if (EventBus.Instance == null)
                {
                    _eventRaised += value;
                }
                else
                {
                    EventBus.Instance.Subscribe(ResourcePath, value);
                }
            }
            remove
            {
                if (EventBus.Instance == null)
                {
                    _eventRaised -= value;
                }
                else
                {
                    EventBus.Instance.Unsubscribe(ResourcePath, value);
                }
            }
        }

        public void Subscribe(Action action)
        {
            EventBus.Instance.Subscribe(ResourcePath, action);
        }

        public void Unsubscribe(Action action)
        {
            EventBus.Instance.Unsubscribe(ResourcePath, action);
        }


        public void SendEvent()
        {
            if (EventBus.Instance == null)
            {
                _eventRaised?.Invoke();
            }
            else
            {
                EventBus.Instance.SendEvent(ResourcePath);
            }
        }
    }
}