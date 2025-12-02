using Godot;
using System;

namespace GameSystems.Event;

[GlobalClass]
public partial class EventBus : Node
{
    public static EventBus Instance { get; private set; }
    public static EventBus GetInstance() => Instance;

    private readonly EventPool _eventPool = new();

    public int EventHandlerCount => _eventPool.EventHandlerCount;

    public int EventCount => _eventPool.EventCount;

    public override void _EnterTree()
    {
        base._EnterTree();
        Instance = this;
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        _eventPool.Close();
    }

    public int Count(string key)
    {
        return _eventPool.Count(key);
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        _eventPool.Process(delta);
    }

    public bool HasEvent(string key, Delegate d)
    {
        return _eventPool.HasEvent(key, d);
    }

    public void Subscribe(string key, Action action)
    {
        _eventPool.Subscribe(key, action);
    }

    public void Subscribe<T>(string key, Action<T> action)
    {
        _eventPool.Subscribe(key, action);
    }

    public void Subscribe<T1, T2>(string key, Action<T1, T2> action)
    {
        _eventPool.Subscribe(key, action);
    }

    public void Unsubscribe(string key, Action action)
    {
        _eventPool.Unsubscribe(key, action);
    }

    public void Unsubscribe<T>(string key, Action<T> action)
    {
        _eventPool.Unsubscribe(key, action);
    }

    public void Unsubscribe<T1, T2>(string key, Action<T1, T2> action)
    {
        _eventPool.Unsubscribe(key, action);
    }

    public void SendEvent(string key)
    {
        _eventPool.SendEvent(key);
    }

    public void SendEvent<T>(string key, T arg)
    {
        _eventPool.SendEvent(key, arg);
    }

    public void SendEvent<T1, T2>(string key, T1 arg1, T2 arg2)
    {
        _eventPool.SendEvent(key, arg1, arg2);
    }
}
