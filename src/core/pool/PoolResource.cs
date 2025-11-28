using Godot;
using System;
using System.Collections.Generic;
using GameSystems.Factory;

namespace GameSystems.Pool;

public abstract partial class PoolResource<T> : Resource, IPool<T>
{
    private readonly Stack<T> _available = new();

    protected Stack<T> Available => _available;
    public abstract IFactory<T> Factory { get; set; }

    public virtual T Request()
    {
        return Available.Count > 0 ? Available.Pop() : Create();
    }

    public virtual void Return(T member)
    {
        Available.Push(member);
    }

    protected virtual T Create()
    {
        return Factory.Create();
    }
}
