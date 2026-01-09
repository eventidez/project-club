using Godot;
using System.Collections.Generic;

namespace GameSystems.RuntimeSet;

[Tool, GlobalClass]
public partial class ResourceGroup : Resource
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
