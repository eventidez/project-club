using Godot;
using System;

namespace GameSystems.Event;

[GlobalClass]
public partial class IntEventChannelListener : Node
{
    [Signal] public delegate void ResponseEventHandler(int parameter);

    [Export] private IntEventChannel _eventChannel;

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

    public void OnEventRaised(int parameter)
    {
        EmitSignal(SignalName.Response, parameter);
    }
}
