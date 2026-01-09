using Godot;
using System;
using System.Collections.Generic;

namespace GameSystems.RuntimeSet;

public static class ResourceGroupExtensions
{
     public static T Get<T>(this ResourceGroup group, string resourceName) where T : class
    {
        foreach (var resource in group.Resources)
        {
            if (resource.ResourceName == resourceName)
            {
                return resource as T;
            }
        }

        return default;
    }

    public static IEnumerable<T> GetResources<T>(this ResourceGroup group)
        where T : Resource
    {
        foreach (var resource in group.Resources)
        {
            if (resource is not T t)
            {
                continue;
            }

            yield return t;
        }
    }

    public static IEnumerable<T> GetResources<T>(this ResourceGroup group, Func<T, bool> predicate)
        where T : Resource
    {
        foreach (var resource in group.Resources)
        {
            if (resource is not T t)
            {
                continue;
            }

            if (predicate(t) == false)
            {
                continue;
            }

            yield return t;
        }
    }
}
