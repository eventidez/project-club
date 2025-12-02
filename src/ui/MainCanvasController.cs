using Godot;
using System;
using GameSystems.Event;

namespace Game.UI;

public partial class MainCanvasController : CanvasLayer
{
    [Export] private ComputerView _computerView = default;
    [Export] private VoidEventChannel _computerAreaPressed = default;
    private Godot.Collections.Array<BaseView> _views = [];

    public override void _EnterTree()
    {
        base._EnterTree();
        _computerAreaPressed.EventRaised += _computerView.Open;
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        _computerAreaPressed.EventRaised -= _computerView.Open;
    }
}
