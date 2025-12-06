using Godot;
using System;

namespace Game;

public partial class GameRoleForm : Node2D, IGameRoleForm
{
    // [Export] private MouseArea2d _mouseArea2D = default;
    [Export] private RoleMenuView _menuView = default;

    private bool _isSelect = false;

    public Action<IGameRoleForm> RolePressed;

    public bool IsSelect
    {
        get => _isSelect;
        set
        {
            _isSelect = value;
            _menuView.Visible = _isSelect;
        }
    }

    public override void _Ready()
    {
        base._Ready();
        // _mouseArea2D.MouseEntered = OnAreaMouseEntered;
        // _mouseArea2D.MouseExited = OnAreaMouseExited;
        EventTriggerListener.Get(this).Pressed = OnRolePressed;
    }

    // private void OnAreaMouseEntered()
    // {
    //     _menuView.Visible = true;
    //     // _menuView.Open();
    // }

    // private void OnAreaMouseExited()
    // {
    //     _menuView.Visible = false;
    // }

    private void OnRolePressed()
    {
        RolePressed?.Invoke(this);
        // _menuView.Visible = true;
        // _menuView.Open();
    }
}
