using Godot;
using System;

[GlobalClass]
public partial class ItemListHelper : Node
{
    [Export] private Node _content = default;
    [Export] private GameSystems.Pool.GodotNodePoolResource _itemPool = default;

    private Godot.Collections.Array<Node> _items = [];

    public Node Content { get => _content; set => _content = value; }

    public override void _Ready()
    {
        base._Ready();
        if (_content == null)
        {
            _content = GetParent();
        }
    }

    public void Open(int count, Action<Node, int> callback)
    {
        foreach (var item in _items)
        {
            _content.RemoveChild(item);
            _itemPool.Return(item);
        }

        _items.Clear();

        for (int i = 0; i < count; i++)
        {
            var item = _itemPool.Request();

            callback(item, i);
            _items.Add(item);
            _content.AddChild(item);
        }
    }
}
