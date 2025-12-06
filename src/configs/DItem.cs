using Godot;

namespace Game;

public partial class DItem : DataBase
{
    [Export] public int Money { get; set; }
    [Export] public string SettingName { get; set; }
}