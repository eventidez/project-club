using Godot;
using System;

public partial class ComputerTabsView : Control
{
    [Export] private Control[] _tabs = default;
    [Export] private GameSystems.Event.IntEventChannel _tabChanged = default;

    public override void _EnterTree()
    {
        base._EnterTree();

        for (int i = 0; i < _tabs.Length; i++)
        {
            var index = i;
        }
    }

    public void ToggleChanged(int index)
    {
        _tabChanged.SendEvent(index);
        GD.Print(index);
    }
}
