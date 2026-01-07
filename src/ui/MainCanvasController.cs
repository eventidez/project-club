using Godot;
using System;
using Chickensoft.AutoInject;
using Chickensoft.GodotNodeInterfaces;
using Chickensoft.Introspection;
using GameSystems.Event;

namespace Game.UI;

public interface IMainCanvasController : ICanvasLayer
{
}

[Meta(typeof(IAutoNode))]
public partial class MainCanvasController : CanvasLayer, IMainCanvasController, IProvide<MainCanvasController>
{
    public override void _Notification(int what) => this.Notify(what);

    [Export] private MainView _mainView = default;
    [Export] private ComputerView _computerView = default;
    [Export] private VoidEventChannel _computerAreaPressed = default;
    private Godot.Collections.Array<BaseView> _views = [];

    MainCanvasController IProvide<MainCanvasController>.Value() => this;

    public override void _EnterTree()
    {
        base._EnterTree();
        this.Provide();
        // _computerAreaPressed.EventRaised += _computerView.Open;
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        // _computerAreaPressed.EventRaised -= _computerView.Open;
    }

    public override void _Ready()
    {
        base._Ready();
        for (int i = 0; i < GetChildCount(); i++)
        {
            var child = GetChild(i);
            if (child is not BaseView view)
            {
                continue;
            }

            view.Visible = false;
        }
    }

    public void OpenComputerView()
    {
        _computerView.Open();
    }

    public void OpenRoleMenu()
    {

    }

    public void OnResolved()
    {
        _mainView.Open();
    }

    public void AddView(BaseView view)
    {
        _views.Add(view);
        // GD.Print(_views.Count);
    }

    public void RemoveView(BaseView view)
    {
        _views.Remove(view);
    }
}
