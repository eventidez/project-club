using Godot;
using System;
using System.Collections.Generic;

public static class GodotExtension
{
    public static T FindParent<T>(this Node node)
    {
        if (node == null)
        {
            GD.PushWarning($"Can not find parent '{typeof(T)}' ");
            return default;
        }

        if (node is T result)
        {
            return result;
        }

        return FindParent<T>(node.GetParent());
    }

    public static bool TryGetChild<T>(this Node node, out T child)
    {
        for (int i = 0; i < node.GetChildCount(); i++)
        {
            if (node.GetChild(i) is T result)
            {
                child = result;
                return true;
            }
        }

        child = default;
        return false;
    }

    public static T FindChild<T>(this Node node)
    {
        if (TryGetChild(node, out T result))
        {
            return result;
        }

        GD.PushWarning($"Node '{node.GetPath()}': can not find {typeof(T)} ");
        return default;
    }

    public static Node FindChild(this Node node, Func<Node, bool> predicate)
    {
        for (int i = 0; i < node.GetChildCount(); i++)
        {
            var child = node.GetChild(i);
            if (predicate(child))
            {
                return child;
            }
        }

        return default;
    }

    public static T As<T>(this GodotObject go)
    {
        if (go is T result)
        {
            return result;
        }

        return default;
    }

    public static T Find<T>(this IEnumerable<Node> nodes)
    {
        foreach (var node in nodes)
        {
            if (node is T result)
            {
                return result;
            }
        }

        GD.PushWarning($"Can not find {typeof(T)} ");
        return default;
    }
}
