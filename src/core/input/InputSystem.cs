using Godot;
using System;
using System.Collections.Generic;

namespace GameSystems.Input;

public partial class InputSystem : Node
{
    private static Godot.Collections.Array<Node> _mouseNodes = [];

    public static ICollection<Node> MouseNodes => _mouseNodes;

    // 拖拽这里写，或许
    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
    //     GD.Print(@event);
    // }

    // public override void _UnhandledInput(InputEvent @event)
    // {
    //     base._UnhandledInput(@event);
        if (@event is InputEventMouseButton e)
        {
            if (e.ButtonIndex == MouseButton.Left && e.Pressed == false)
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
            }
            // GD.Print(e.GetType(), "   >", e.Pressed);
        }
    }

    // public override void _ShortcutInput(InputEvent @event)
    // {
    //     base._ShortcutInput(@event);
    //     GD.Print(@event.GetType());
    // }
}
