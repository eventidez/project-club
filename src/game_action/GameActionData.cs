using Godot;
using Godot.Collections;
using System;

public partial class GameActionData : Resource
{
    [Export] public int Day { get; set; }
    [Export] public Array<string> Conditions { get; set; } = [];
    [Export] public Array<string> Effects { get; set; } = [];
}
