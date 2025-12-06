using Godot;

namespace Game;

public partial class DataSetting : DataBase
{
    [Export] public string Type { get; set; }
    [Export] public string InitialValue { get; set; }
    [Export] public string MinValue { get; set; }
    [Export] public string MaxValue { get; set; }
}
