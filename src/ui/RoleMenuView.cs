using Godot;
using System;

namespace Game;

public partial class RoleMenuView : Control
{
    [Export] private Godot.Collections.Array<Control> _buttonContainers = [];

    public override void _Ready()
    {
        base._Ready();
        Visible = false;
        MouseEntered += () => GD.Print(GetPath());
    }

    public void Open(IGameRoleForm roleForm = default)
    {
        Visible = true;
        UpdateContainer(0);
    }

    private void UpdateContainer(int index)
    {
        for (int i = 0; i < _buttonContainers.Count; i++)
        {
            _buttonContainers[i].Visible = i == index;
        }
    }

    public void OnClothingButtonPressed()
    {
        UpdateContainer(1);
    }
}
