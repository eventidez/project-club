using System;
using Chickensoft.AutoInject;
using Chickensoft.Introspection;
using Godot;

namespace Game.UI;

[Meta(typeof(IAutoNode))]
public partial class BaseView : Control
{
    public override void _Notification(int what) => this.Notify(what);

    [Dependency] public MainCanvasController Controller => this.DependOn<MainCanvasController>();

    public void Open()
    {
        Visible = true;
        Controller.AddView(this);

        OnOpen();
    }

    public void Close()
    {
        Visible = false;
        Controller.RemoveView(this);
    }

    public virtual void OnOpen() { }
}
