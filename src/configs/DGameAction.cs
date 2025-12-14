using Godot;
using Godot.Collections;

namespace Game;

public partial class DGameAction : Resource
{
    [Export] public int Day { get; set; }
    [Export] public int MaxCount { get; set; }
    [Export] public string MoneyCondition { get; set; }
    [Export] public Array<string> Conditions { get; set; }
    [Export] public Array<string> Effects { get; set; }
}
