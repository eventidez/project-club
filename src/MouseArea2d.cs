using Godot;
using System;

public partial class MouseArea2d : Area2D
{
    public override void _MouseEnter()
    {
        base._MouseEnter();
        GD.Print("Enter");
    }

    public override void _MouseExit()
    {
        base._MouseExit();
        GD.Print("Exit");
    }
}
