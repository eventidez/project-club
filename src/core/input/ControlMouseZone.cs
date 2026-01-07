using Godot;
using System;
using GameSystems.RuntimeSet;

namespace GameSystems.Input;

[GlobalClass]
public partial class ControlMouseZone : Node
{
    private Control _parent;

    public override void _EnterTree()
    {
        base._EnterTree();
        _parent = GetParent<Control>();
        _parent.MouseEntered += OnParentMouseEntered;
        _parent.MouseExited += OnParentMouseExited;
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        _parent.MouseEntered -= OnParentMouseEntered;
        _parent.MouseExited -= OnParentMouseExited;
    }

    // public override string[] _GetConfigurationWarnings()
    // {
    //     return base._GetConfigurationWarnings();
    // }

    private void OnParentMouseEntered()
    {
        InputSystem.MouseNodes.Add(GetParent());
    }

    private void OnParentMouseExited()
    {
        InputSystem.MouseNodes.Remove(GetParent());
    }
}
