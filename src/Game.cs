using Chickensoft.AutoInject;
using Chickensoft.Introspection;
using Godot;
using System;

namespace Game;

[Meta(typeof(IAutoNode))]
public partial class Game : Node2D
{
    public override void _Notification(int what) => this.Notify(what);
    [Dependency] public ClassA A => this.DependOn<ClassA>();

    public override void _EnterTree()
    {
        base._EnterTree();
        GD.Print("_EnterTree ", GetPath());
    }

    public override void _Ready()
    {
        base._Ready();
        GD.Print("_Ready ", GetPath());
    }

    public void OnEnterTree()
    {
        GD.Print("OnEnterTree ", GetPath());
    }

    public void Setup()
    {
        GD.Print("Setup ", GetPath());
        GD.Print("Setup: ", A);
    }

    public void OnResolved()
    {
        GD.Print("OnResolved ", GetPath());
        GD.Print("OnResolved: ", A);

    }
}
