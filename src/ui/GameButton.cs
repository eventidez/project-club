using Godot;
using System;
using GameSystems.Event;

public partial class GameButton : TextureButton
{
    [Signal] public delegate void MousePressedEventHandler();
    [Signal] public delegate void MousePressedAtIndexEventHandler();

    [Export] private VoidEventChannel _pressed = default;
    [Export] private Label _label;
    [Export] private int _index = -1;

    public string Text
    {
        get => _label.Text;
        set => _label.Text = value;
    }
    public int Index
    {
        get => _index;
        set => _index = value;
    }

    public override void _EnterTree()
    {
        base._EnterTree();
        EventTriggerListener.Get(this).Pressed = Press;
        // MouseEntered += () => GD.Print("XXX");
    }
    // public Action MousePressed;

    public void Press()
    {
        _pressed?.SendEvent();
        EmitSignal(SignalName.MousePressed);

        if (Index >= 0)
        {
            EmitSignal(SignalName.MousePressedAtIndex);
        }
    }
}
