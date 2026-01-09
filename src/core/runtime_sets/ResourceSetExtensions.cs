using Godot;
using System;
using System.Collections.Generic;

namespace GameSystems.RuntimeSet;

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

    public static IEnumerable<T> GetResources<T>(this ResourceSet resourceSet)
        where T : Resource
    {
        foreach (var resource in resourceSet.Resources)
        {
            if (resource is not T t)
            {
                continue;
            }

            yield return t;
        }
    }

    public static IEnumerable<T> GetResources<T>(this ResourceSet resourceSet, Func<T, bool> predicate)
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

            yield return t;
        }
    }
}
