using Godot;
using System;
using Chickensoft.AutoInject;
using Chickensoft.Introspection;

namespace Game;

[Meta(typeof(IAutoNode))]
public partial class App : Node, IProvide<string>
{
    public override void _Notification(int what) => this.Notify(what);

    private TestX _test = default;

    string IProvide<string>.Value() => "APP";

    public override void _EnterTree()
    {
        base._EnterTree();
        this.Provide();
        // GD.PrintErr(GetPath(), "  _EnterTree");
    }

    public override void _Ready()
    {
        base._Ready();
        // GD.PrintErr(GetPath(), "  _Ready");

    }

    public void AddGame()
    {
        if (_test == null)
        {
            _test = new TestX();
        }

        if (_test.GetParent() == null)
        {
            AddChild(_test);
        }

        // GD.PrintErr(GetPath(), "  AddGame");
    }

    public void RemoveGame()
    {
        if (_test == null)
        {
            return;
        }

        if (_test.GetParent() == this)
        {
            RemoveChild(_test);
        }
        // AddChild(new Game());
        // GD.PrintErr(GetPath(), "  AddGame");
    }
}
