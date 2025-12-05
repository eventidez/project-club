using Godot;
using System;
using System.Collections.Generic;

namespace GameSystems.RuntimeSet;

[GlobalClass]
public partial class GodotNodeRuntimeSet : RuntimeSet<Node>
{
    public IEnumerable<T> GetNodes<T>()
    {
        foreach (var item in Items)
        {
            if (item is T t)
            {
                yield return t;
            }
        }
    }
}
