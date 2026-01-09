using Godot;
using System;
using GameSystems.RuntimeSet;

namespace Game;

public partial class DataLoading : Node
{
    [Export] private ResourceGroup _shopItemGroup = default;

    public override void _EnterTree()
    {
        base._EnterTree();
        LoadData();
    }

    public void LoadData()
    {
        var json = ResourceLoader.Load<Json>("res://configs/tables/shop.json");
        json.ToResources<DShopItem>((d) =>
       {
           _shopItemGroup.Add(d);
       });
    }
}
