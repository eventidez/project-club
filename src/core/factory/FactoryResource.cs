using Godot;
using System;

namespace GameSystems.Factory;

public abstract partial class FactoryResource<T> : Resource, IFactory<T>
{
    public abstract T Create();
}
