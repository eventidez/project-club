using Godot;
using System;
using GameSystems.RuntimeSet;

public partial class MouseArea2d : Node
{
    [Export] private Area2D _area2d = default;
    public Action MouseEntered;
    public Action MouseExited;

    public override void _EnterTree()
    {
        base._EnterTree();
        _area2d ??= GetParent<Area2D>();

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
        InputSystem.MouseNodes.Add(GetParent());
    }

    public void OnMouseExited()
    {
        // GD.Print("Exit");
        MouseExited?.Invoke();
        InputSystem.MouseNodes.Remove(GetParent());
    }
}
