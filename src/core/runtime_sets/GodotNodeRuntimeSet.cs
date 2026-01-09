using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

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

public static class GodotNodeRuntimeSetEx
{
    public static T FindNode<T>(this GodotNodeRuntimeSet set, Func<T, bool> predicate)
    {
        return set.GetNodes<T>().FirstOrDefault(predicate);
    }

      public static T[] ToArray<T>(this GodotNodeRuntimeSet set)
    {
        return [.. set.GetNodes<T>()];
    }
}
