using Godot;
using System;

namespace Game.UI;

public partial class MainView : BaseView
{
    public override void _EnterTree()
    {
        base._EnterTree();

        GD.Print(nameof(_EnterTree));
    }

    public override void _Draw()
    {
        base._Draw();

        GD.Print(nameof(_Draw), ": ", Visible);
    }
}
