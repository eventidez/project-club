using Godot;
using System;
using System.Collections.Generic;
using GameSystems.Event;

namespace GameSystems.Input;

public partial class InputSystem : Node
{
    [Export] private VoidEventChannel _emptyMousePressed = default;

    private static Godot.Collections.Array<Node> _mouseNodes = [];

    public static ICollection<Node> MouseNodes => _mouseNodes;

    // 拖拽这里写，或许
    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
        if (Is<InputEventMouseButton>(@event, e => e.ButtonIndex == MouseButton.Left && e.Pressed == false))
        {
            foreach (var mouseNode in MouseNodes)
            {
                if (mouseNode.TryGetChild<EventTriggerListener>(out var child) == false)
                {
                    continue;
                }

                // GD.Print(mouseNode.GetType());
                child.Press();
            }
            // GD.Print(e.GetType(), "   >", e.Pressed);
        }
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        base._UnhandledInput(@event);
        if (Is<InputEventMouseButton>(@event, e => e.ButtonIndex == MouseButton.Left && e.Pressed == false))
        {
            if (MouseNodes.Count > 0)
            {
                return;
            }

            _emptyMousePressed?.SendEvent();
        }
    }

    private bool Is<T>(InputEvent inputEvent, Func<T, bool> predicate)
    {
        if (inputEvent is not T e)
        {
            return false;
        }

        return predicate(e);
    }
}
