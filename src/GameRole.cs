using Godot;
using System;

public partial class GameRole : Node2D
{
    [Export] private MouseArea2d _mouseArea2D;
    [Export] private RoleMenuView _menuView;

    public override void _Ready()
    {
        base._Ready();
        _mouseArea2D.MouseEntered = OnAreaMouseEntered;
        _mouseArea2D.MouseExited = OnAreaMouseExited;
    }

    private void OnAreaMouseEntered()
    {
        _menuView.Visible = true;
    }

    private void OnAreaMouseExited()
    {
        _menuView.Visible = false;
    }
}
