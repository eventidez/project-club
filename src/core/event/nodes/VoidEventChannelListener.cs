using Godot;
using System;

namespace GameSystems.Event;

[GlobalClass]
public partial class VoidEventChannelListener : Node
{
    [Signal] public delegate void ResponseEventHandler();

    [Export] private VoidEventChannel _eventChannel;

    public override void _EnterTree()
    {
        base._EnterTree();

        if (_eventChannel != null)
            _eventChannel.EventRaised += OnEventRaised;
    }

    public override void _ExitTree()
    {
        base._ExitTree();

        if (_eventChannel != null)
            _eventChannel.EventRaised -= OnEventRaised;
    }

    public void OnEventRaised()
    {
        EmitSignal(SignalName.Response);
    }
}
