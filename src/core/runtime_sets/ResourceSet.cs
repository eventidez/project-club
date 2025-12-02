using Godot;
using System;
using System.Collections.Generic;

namespace GameSystems.RuntimeSet;

[Tool, GlobalClass]
public partial class ResourceSet : Resource
{
    [Export] protected Godot.Collections.Array<Resource> _resources = [];
    // private int _serial = 0;

    public IReadOnlyList<Resource> Resources => _resources;

    public void Add(Resource resource)
    {
        if (resource == null || _resources.Contains(resource))
        {
            return;
        }

        _resources.Add(resource);
    }

    public void Remove(Resource resource)
    {
        if (resource == null)
        {
            return;
        }

        _resources.Remove(resource);
    }

    public void Clear()
    {
        _resources.Clear();
    }

    public int Count() => _resources.Count;

    public Resource Get(int index)
    {
        return _resources[index];
    }

    public IEnumerable<Resource> All()
    {
        return _resources;
    }
}

public static class ResourceSetExtensions
{
    public static T Get<T>(this ResourceSet resourceSet, string resourceName) where T : class
    {
        foreach (var resource in resourceSet.Resources)
        {
            if (resource.ResourceName == resourceName)
            {
                return resource as T;
            }
        }

        return default;
    }

    public static IEnumerable<T> GetAll<T>(this ResourceSet resourceSet, Func<T, bool> predicate)
        where T : Resource
    {
        foreach (var resource in resourceSet.Resources)
        {
            if (resource is not T t)
            {
                continue;
            }

            if (predicate(t) == false)
            {
                continue;
            }

            yield return resource as T;
        }
    }
}
