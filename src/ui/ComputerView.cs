using Chickensoft.AutoInject;
using Chickensoft.Introspection;
using Godot;
using System;

namespace Game.UI;

[Meta(typeof(IAutoNode))]
public partial class ComputerView : BaseView
{
    public override void _Notification(int what) => this.Notify(what);

    public override void _Draw()
    {
        base._Draw();
        GD.Print(nameof(_Draw), ": ", Visible);
    }

}
