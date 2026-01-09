using Godot;
using System;

namespace Game;

public partial class ShopGoodsItem : Control
{
    public int Index { get; set; }

    [Export] private Label _nameLabel = default;
    [Export] private Label _priceLabel = default;
    [Export] private CanvasItem _priceContainer = default;

    public void Show(DShopItem itemData)
    {
        _nameLabel.Text = I18NUtil.GetString(itemData.NameText);
    }
}
