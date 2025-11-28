using GameSystems.Factory;
using Godot;
using System;

namespace GameSystems.Pool;

[GlobalClass]
public partial class GodotNodePoolResource : PoolResource<Node>
{
    [Export] public PackedScene Scene { get; set; } = default;
    private IFactory<Node> _factory = default;
    
    public override IFactory<Node> Factory
    {
        get
        {
            _factory ??= new PackedSceneFactoryResource()
            {
                Scene = Scene,
            };

            return _factory;
        }
        set => _factory = value;
    }

    public override void Return(Node member)
    {
        // var parent = member.GetParent();
        // parent.RemoveChild(member);
        base.Return(member);
    }

    protected override Node Create()
    {
        var instance = base.Create();
        return instance;
    }
}
