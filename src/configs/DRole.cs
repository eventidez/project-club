using Godot;
using System;

namespace Game;

public partial class DRole : DataBase
{
    [Export] public string LoveSetting { get; set; }
    [Export] public string Clothing2Setting { get; set; }
    [Export] public string Clothing3Setting { get; set; }
}
