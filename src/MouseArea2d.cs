using Godot;
using System;

public partial class MouseArea2d : Node
{
    [Export] private Area2D _area2d;

    public Action MouseEntered;
    public Action MouseExited;

    public override void _EnterTree()
    {
        base._EnterTree();
        _area2d.MouseEntered += OnMouseEntered;
        _area2d.MouseExited += OnMouseExited;
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        _area2d.MouseEntered -= OnMouseEntered;
        _area2d.MouseExited -= OnMouseExited;
    }

    public void OnMouseEntered()
    {
        // GD.Print("Enter");
        MouseEntered?.Invoke();
    }

    public void OnMouseExited()
    {
        // GD.Print("Exit");
        MouseExited?.Invoke();
    }
}
