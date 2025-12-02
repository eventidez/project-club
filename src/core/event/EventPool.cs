using Godot;
using System;
using System.Collections;
using System.Collections.Generic;

namespace GameSystems;

public partial class EventPool : GodotObject
{
    private readonly Dictionary<string, LinkedList<Delegate>> _eventHandlers;
    private readonly Queue<string> _events;
    private readonly Dictionary<object, LinkedListNode<Delegate>> _cachedNodes;
    private readonly Dictionary<object, LinkedListNode<Delegate>> _tempNodes;

    public EventPool()
    {
        _eventHandlers = [];
        _events = new Queue<string>();
        _cachedNodes = [];
        _tempNodes = [];
    }

    public int EventHandlerCount => _eventHandlers.Count;

    public int EventCount => _events.Count;

    public void Process(double delta)
    {
        // lock (_events)
        // {
        //     while (_events.Count > 0)
        //     {
        //         var eventNode = _events.Dequeue();
        //         HandleEvent(eventNode.Key, eventNode.Sender, eventNode.EventArgs);
        //     }
        // }
    }

    public void Close()
    {
        Clear();
        _eventHandlers.Clear();
        _cachedNodes.Clear();
        _tempNodes.Clear();
    }

    public void Clear()
    {
        lock (_events)
        {
            _events.Clear();
        }
    }

    public int Count(string key)
    {
        if (_eventHandlers.TryGetValue(key, out var list))
        {
            return list.Count;
        }

        return 0;
    }

    public bool HasEvent(string key, Delegate handler)
    {
        if (handler == null)
        {
            throw new Exception("Event handler is invalid.");
        }

        if (_eventHandlers.TryGetValue(key, out var list) == false)
        {
            return false;
        }

        return list.Contains(handler);
    }

    public void Subscribe(string key, Delegate handler)
    {
        if (handler == null)
        {
            throw new Exception("Event handler is invalid.");
        }

        if (_eventHandlers.TryGetValue(key, out var list) == false)
        {
            list = [];
            _eventHandlers[key] = list;
        }

        list.AddLast(handler);
    }

    public void Unsubscribe(string key, Delegate handler)
    {
        if (handler == null)
        {
            throw new Exception("Event handler is invalid.");
        }

        if (_cachedNodes.Count > 0)
        {
            foreach (var cachedNode in _cachedNodes)
            {
                if (cachedNode.Value != null && cachedNode.Value.Value == handler)
                {
                    _tempNodes.Add(cachedNode.Key, cachedNode.Value.Next);
                }
            }

            if (_tempNodes.Count > 0)
            {
                foreach (var cachedNode in _tempNodes)
                {
                    _cachedNodes[cachedNode.Key] = cachedNode.Value;
                }

                _tempNodes.Clear();
            }
        }

        if (_eventHandlers.TryGetValue(key, out var list) == false || list.Remove(handler) == false)
        {
            throw new Exception($"Event '{key}' not exists specified handler.");
        }
    }

    public void SendEvent(string key)
    {
        if (_eventHandlers.TryGetValue(key, out var list) == false)
        {
            return;
        }

        var current = list.First;
        while (current != null)
        {
            _cachedNodes[key] = current.Next;
            if (current.Value is Action action)
            {
                action();
            }

            current = _cachedNodes[key];
        }

        _cachedNodes.Remove(key);
    }

    public void SendEvent<T>(string key, T arg)
    {
        if (arg == null)
        {
            throw new Exception("Event arg is invalid.");
        }

        HandleEvent<Action<T>>(key, (action) => action(arg));
    }

    public void SendEvent<T1, T2>(string key, T1 arg1, T2 arg2)
    {
        if (arg1 == null)
        {
            throw new Exception("Event arg is invalid.");
        }

        HandleEvent<Action<T1, T2>>(key, (action) => action(arg1, arg2));
    }

    private void HandleEvent<T>(string key, Action<T> callback)
    {
        if (_eventHandlers.TryGetValue(key, out var list) == false)
        {
            return;
        }

        var current = list.First;
        while (current != null)
        {
            _cachedNodes[key] = current.Next;
            if (current.Value is T t)
            {
                callback(t);
            }

            current = _cachedNodes[key];
        }

        _cachedNodes.Remove(key);
    }

    // private void HandleEvent(string key, object sender, T e)
    // {
    //     bool noHandlerException = false;
    //     if (_eventHandlers.TryGetValue(key, out var list))
    //     {
    //         var current = list.First;
    //         while (current != null)
    //         {
    //             _cachedNodes[e] = current.Next;
    //             current.Value(sender, e);
    //             current = _cachedNodes[e];
    //         }

    //         _cachedNodes.Remove(e);
    //     }

    //     if (noHandlerException)
    //     {
    //         throw new Exception($"Event '{key}' not allow no handler.");
    //     }
    // }

    // private sealed partial class Event : RefCounted
    // {
    //     private string _key;
    //     private object _sender;
    //     private T _eventArgs;

    //     public Event(string key, object sender, T eventArgs)
    //     {
    //         this._key = key;
    //         _sender = sender;
    //         _eventArgs = eventArgs;
    //     }

    //     public object Sender => _sender;
    //     public T EventArgs => _eventArgs;

    //     public string Key => _key;

    //     public void Clear()
    //     {
    //         _key = default;
    //         _sender = default;
    //         _eventArgs = default;
    //     }
    // }
}
