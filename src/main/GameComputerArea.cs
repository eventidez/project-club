using Godot;
using System;
using GameSystems.Event;

namespace Game;

public partial class GameComputerArea : Area2D
{
    [Signal] public delegate void MousePressedEventHandler();
    [Export] private VoidEventChannel _pressed = default;

    public override void _EnterTree()
    {
        base._EnterTree();
    }

    public override void _Ready()
    {
        base._Ready();
        EventTriggerListener.Get(this).Pressed = OnPressed;

    }

    private void OnPressed()
    {
        // _pressed.SendEvent();
        EmitSignal(SignalName.MousePressed);
    }
}
