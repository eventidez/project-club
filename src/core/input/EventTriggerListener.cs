using Godot;
using System;

public partial class EventTriggerListener : Node
{
    // public Action MouseEntered;
    // public Action MouseExited;
    // public Action<PointerEventData> DragEntered;
    // public Action<PointerEventData> DragExited;
    // public Action<PointerEventData> Droped;

    public Action Pressed;

    public static EventTriggerListener Get(Node node)
    {
        if (node.TryGetChild<EventTriggerListener>(out var listener) == false)
        {
            listener = new EventTriggerListener();
            node.AddChild(listener);
        }

        return listener;
    }

    public void Press()
    {
        Pressed?.Invoke();
    }

    // public void EnterDrag(PointerEventData eventData)
    // {
    //     DragEntered?.Invoke(eventData);
    // }

    // public void ExitDrag(PointerEventData eventData)
    // {
    //     DragExited?.Invoke(eventData);
    // }

    // public void Drop(PointerEventData eventData)
    // {
    //     Droped?.Invoke(eventData);
    // }
}
