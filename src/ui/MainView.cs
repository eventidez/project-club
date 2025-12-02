using Godot;
using System;
using Chickensoft.AutoInject;
using Chickensoft.Introspection;

namespace Game.UI;

[Meta(typeof(IAutoNode))]
public partial class MainView : BaseView
{
    public override void _Notification(int what) => this.Notify(what);

    public override void _EnterTree()
    {
        base._EnterTree();

        GD.Print(nameof(_EnterTree));
    }

    public void OnResolved()
    {
    }
}
