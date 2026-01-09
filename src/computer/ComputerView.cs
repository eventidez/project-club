using Chickensoft.AutoInject;
using Chickensoft.Introspection;
using Godot;
using System;

namespace Game;

[Meta(typeof(IAutoNode))]
public partial class ComputerView : BaseView
{
    public override void _Notification(int what) => this.Notify(what);

    [Export] private int _firstContentIndex = 0;
    [Export] private Control[] _contents = [];

    public override void _Draw()
    {
        base._Draw();
        GD.Print(nameof(_Draw), ": ", Visible);
    }

    public override void OnOpen()
    {
        base.OnOpen();

        OpenContent(_firstContentIndex);
    }

    private void OpenContent(int index)
    {
        for (int i = 0; i < _contents.Length; i++)
        {
            _contents[i].Visible = i == index;
        }
    }
}
