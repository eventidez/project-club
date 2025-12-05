using Godot;
using System;

namespace GameSystems.RuntimeSet;

[GlobalClass]
public partial class GodotNodeZone : Node
{
    [Export] private GodotNodeRuntimeSet _nodeSet = default;


    public override void _EnterTree()
    {
        base._EnterTree();
        _nodeSet?.Add(GetParent());
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        _nodeSet?.Remove(GetParent());
    }
}
