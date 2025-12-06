using Godot;
using System;
using System.Collections.Generic;
using Chickensoft.AutoInject;
using Chickensoft.Introspection;
using GameSystems.RuntimeSet;

namespace Game;

public interface IMainGameRepo
{
    void NextDay();
}

[Meta(typeof(IAutoNode))]
public partial class MainGame : Node, IMainGameRepo, IProvide<IMainGameRepo>, IProvide<MainGameSet>
{
    public override void _Notification(int what) => this.Notify(what);

    [Export] private GodotNodeRuntimeSet _roleNodeSet = default;
    private MainGameSet _mainGameSet = default;
    private List<IGameRoleForm> _roles = [];

    MainGameSet IProvide<MainGameSet>.Value() => _mainGameSet;
    IMainGameRepo IProvide<IMainGameRepo>.Value() => this;

    public override void _EnterTree()
    {
        _mainGameSet = new MainGameSet();
        this.Provide();
    }

    public override void _ExitTree()
    {
        base._ExitTree();
    }

    public override void _Ready()
    {
        base._Ready();

        _roles.Clear();
        foreach (var role in _roleNodeSet.GetNodes<GameRoleForm>())
        {
            role.RolePressed = OnGameRolePressed;
            _roles.Add(role);
        }
    }

    public void NextDay()
    {
        _mainGameSet.Day += 1;
    }

    private void OnGameRolePressed(IGameRoleForm roleForm)
    {
        if (roleForm.IsSelect)
        {
            return;
        }

        GD.Print($"Press: {roleForm}");
        foreach (var role in _roles)
        {
            role.IsSelect = role == roleForm;
        }
    }
}
