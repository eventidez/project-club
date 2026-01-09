using Godot;
using System;

namespace Game;

public partial class ScheduleGoodsItem : Control
{
    [Export] private Label _nameLabel = default;

    public DItem Data { get; set; }

    public override bool _CanDropData(Vector2 atPosition, Variant data)
    {
        return base._CanDropData(atPosition, data);
    }

    public override Variant _GetDragData(Vector2 atPosition)
    {
        return Data;
    }
}
