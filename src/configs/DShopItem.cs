using Godot;

namespace Game;

public partial class DShopItem : DataBase
{
    [Export] public string NameText { get; set; }
    [Export] public string Type { get; set; }
    [Export] public int Price { get; set; }
    [Export] public string Condition { get; set; }
    [Export] public string Effect { get; set; }
}