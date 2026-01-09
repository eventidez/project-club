using Godot;
using System;

namespace Game;

public partial class GameRoleForm : Node2D, IGameRoleForm
{
    [Export] private string _roleId = default;
    // [Export] private MouseArea2d _mouseArea2D = default;
    [Export] private RoleMenuView _menuView = default;
    [Export] private Sprite2D _bodySprite = default;
    [Export] private Sprite2D _faceSprite = default;

    private bool _isSelect = false;

    public string RoleId => _roleId;
    public DRole Data { get; set; }
    public Action<IGameRoleForm> RolePressed;

    public bool IsSelect
    {
        get => _isSelect;
        set
        {
            _isSelect = value;
            if (_isSelect)
            {
                _menuView.Open();
            }
            else
            {
                _menuView.Close();
            }
        }
    }

    public override void _Ready()
    {
        base._Ready();
        // _mouseArea2D.MouseEntered = OnAreaMouseEntered;
        // _mouseArea2D.MouseExited = OnAreaMouseExited;
        _menuView.CancelPressed = () => IsSelect = false;
        EventTriggerListener.Get(this).Pressed = OnRolePressed;
    }

    public void SetBody(string spriteName)
    {
        _bodySprite.Texture = ResourceLoader.Load<Texture2D>($"res://art/roles/{spriteName}.png");
    }

    public void SetFace(string spriteName)
    {
        _faceSprite.Texture = ResourceLoader.Load<Texture2D>($"res://art/roles/{spriteName}.png");
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
