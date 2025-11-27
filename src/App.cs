using Chickensoft.AutoInject;
using Chickensoft.Introspection;
using Godot;
using System;

namespace Game;

public class ClassA
{
    public ClassA()
    {
        // GD.Print(GetType());
    }
}

[Meta(typeof(IAutoNode))]
public partial class App : Node, IProvide<string>
{
    public override void _Notification(int what) => this.Notify(what);

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
        // AddChild(new Game());
        // GD.PrintErr(GetPath(), "  AddGame");
    }
}
