using Godot;
using System;

public partial class RoleClothingButton : Node
{
    [Signal] public delegate void MousePressedEventHandler();

        [Export] private Label _label;

    public string Text
    {
        get => _label.Text;
        set => _label.Text = value;
    }

}
