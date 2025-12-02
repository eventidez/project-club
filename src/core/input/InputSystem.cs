using Godot;
using System;
using System.Collections.Generic;

public partial class InputSystem : Node
{
    private static Godot.Collections.Array<Node> _mouseNodes = [];

    public static ICollection<Node> MouseNodes => _mouseNodes;

    public override void _UnhandledInput(InputEvent @event)
    {
        base._UnhandledInput(@event);

        if (@event is InputEventMouseButton e)
        {
            if (e.ButtonIndex == MouseButton.Left && e.Pressed == false)
            {
                foreach (var mouseNode in _mouseNodes)
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
}
