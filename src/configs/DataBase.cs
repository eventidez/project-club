using Godot;

namespace Game;

public partial class DataBase : Resource
{
    [Export] public string Id { get; set; }
    [Export] public string Name { get; set; }
}
