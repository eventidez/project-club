using Godot;
using System;

public partial class GameButton : TextureButton
{
    // [Signal] public delegate void MousePressedEventHandler();

    [Export] private Label _label;

    public string Text
    {
        get => _label.Text;
        set => _label.Text = value;
    }

    public override void _EnterTree()
    {
        base._EnterTree();
        // EventTriggerListener.Get(this).Pressed = Press;
        // MouseEntered += () => GD.Print("XXX");
    }
    // public Action MousePressed;

    public void Press()
    {
        // EmitSignal(SignalName.MousePressed);
    }
}
