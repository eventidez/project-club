using Godot;
using System;

public partial class MouseControl : Node
{
    private Control _parent;

    public override void _EnterTree()
    {
        base._EnterTree();
        _parent = GetParent<Control>();
        _parent.MouseEntered += OnMouseEntered;
        _parent.MouseExited += OnMouseExited;
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        _parent.MouseEntered -= OnMouseEntered;
        _parent.MouseExited -= OnMouseExited;
    }

    private void OnMouseEntered()
    {
        GD.Print("Enter");
    }

    private void OnMouseExited()
    {
        GD.Print("Exit");
    }
}
