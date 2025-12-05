using Godot;
using System;
using System.Collections.Generic;

namespace GameSystems.RuntimeSet;

public abstract partial class RuntimeSet<T> : Resource where T : GodotObject
{
    private List<T> _items = [];

    public IReadOnlyList<T> Items => _items;
    public Action ItemsChanged;

    public void Add(T thing)
    {
        if (thing == null || _items.Contains(thing))
        {
            return;
        }

        _items.Add(thing);
        ItemsChanged?.Invoke();
    }

    public void Remove(T thing)
    {
        if (thing == null)
        {
            return;
        }

        _items.Remove(thing);
        ItemsChanged?.Invoke();
    }

    public int Count() => _items.Count;

    public T Get(int index)
    {
        return _items[index];
    }

    public IEnumerable<T> GetAll()
    {
        return _items;
    }
}
