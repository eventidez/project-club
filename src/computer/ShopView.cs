using Godot;
using System;
using GameSystems.RuntimeSet;
using System.Linq;

namespace Game;

public partial class ShopView : Control
{
    [Export] private ResourceGroup _shopItemDataGroup = default;
    [Export] private GodotNodeRuntimeSet _goodsItemGroup = default;
    private ShopGoodsItem[] _goodsItems;
    private DShopItem[] _goodsItemDatas;

    public override void _Ready()
    {
        base._Ready();

        _goodsItems = new ShopGoodsItem[_goodsItemGroup.Count()];
        _goodsItems = _goodsItemGroup.ToArray<ShopGoodsItem>();
        _goodsItemDatas = [.. _shopItemDataGroup.GetResources<DShopItem>((item) => item.Type == "goods")];

        for (int i = 0; i < _goodsItems.Length; i++)
        {
            var goodsItem = _goodsItems[i];
            goodsItem.Index = i;
            if (i >= _goodsItemDatas.Length)
            {
                goodsItem.Visible = false;
                continue;
            }

            goodsItem.Visible = true;
            goodsItem.Show(_goodsItemDatas[i]);
   }
    }
}
