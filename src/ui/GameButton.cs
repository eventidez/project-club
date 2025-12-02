using Godot;
using System;

public partial class GameButton : TextureButton
{
    [Export] private Label _label;

    public string Text
    {
        get => _label.Text;
        set => _label.Text = value;
    }
}
