using Godot;
using System;

namespace Game;

public partial class RoleMenuView : Control
{
    [Export] private Control[] _backgrounds = [];
    [Export] private Godot.Collections.Array<Control> _buttonContainers = [];

    private int _containerIndex = 0;

    public Action CancelPressed;

    public override void _EnterTree()
    {
        base._EnterTree();
    }

    public override void _ExitTree()
    {
        base._ExitTree();
    }

    public override void _Ready()
    {
        base._Ready();
        Visible = false;
    }

    public void Open(IGameRoleForm roleForm = default)
    {
        Visible = true;
        UpdateContainer(0);
    }

    public void Close()
    {
        Visible = false;
    }

    private void Cancel()
    {
        if (_containerIndex == 0)
        {
            CancelPressed?.Invoke();
            return;
        }

        UpdateContainer(_containerIndex - 1);
    }

    private void UpdateContainer(int index)
    {
        _containerIndex = index;
        for (int i = 0; i < _buttonContainers.Count; i++)
        {
            _buttonContainers[i].Visible = i == index;
        }

        for (int i = 0; i < _backgrounds.Length; i++)
        {
            _backgrounds[i].Visible = i == index;
        }
    }

    public void OnClothingButtonPressed()
    {
        UpdateContainer(1);
    }
}
