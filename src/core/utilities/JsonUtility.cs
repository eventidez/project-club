using Godot;
using System;

public static class JsonUtility
{
    public static void SetResource(this Json json, Resource res)
    {
        foreach (var item in json.Data.AsGodotDictionary<string, Variant>())
        {
            res.Set(item.Key, item.Value);
        }
    }

    public static void ToResources<T>(this Json json, Action<T> callback) where T : Resource, new()
    {
        foreach (var dict in json.Data.AsGodotArray<Godot.Collections.Dictionary<string, Variant>>())
        {
            var res = new T();
            foreach (var item in dict)
            {
                res.Set(item.Key, item.Value);
            }

            if (dict.TryGetValue("Id", out var variant))
            {
                res.ResourceName = variant.AsString();
            }

            callback(res);
        }
    }
}
