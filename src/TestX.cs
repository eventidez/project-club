using Godot;
using System;
using Chickensoft.AutoInject;
using Chickensoft.Introspection;

[Meta(typeof(IAutoNode))]
public partial class TestX : Node
{
    public override void _Notification(int what) => this.Notify(what);

    public void OnResolved()
    {
        GD.Print("ADD: ", GetType().Name);
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        GD.Print("Remove: ", GetType().Name);
    }
}
