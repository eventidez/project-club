using Godot;

namespace GameSystems.Factory;

[GlobalClass]
public partial class PackedSceneFactoryResource : FactoryResource<Node>
{
    [Export] public PackedScene Scene { get; set; }

    public override Node Create()
    {
        return Scene.Instantiate();
    }
}
