using Godot;
using System;
using System.Collections.Generic;
using Chickensoft.AutoInject;
using Chickensoft.Introspection;

namespace Game;

public interface IRoleForm
{

}

[Meta(typeof(IAutoNode))]
public partial class MainGame : Node
{
    public override void _Notification(int what) => this.Notify(what);

    private List<IRoleForm> _roles = [];

    public override void _Ready()
    {
        base._Ready();

        _roles.Clear();
        foreach (var child in GetChildren())
        {
            if (child is not GameRoleForm role)
            {
                continue;
            }

            role.RolePressed = OnGameRolePressed;
            _roles.Add(role);
        }
    }

    private void OnGameRolePressed(IRoleForm roleForm)
    {
        GD.Print($"Press: {roleForm}");
    }
}
