using Godot;

namespace Game.UI;

public partial class BaseView : Control
{
    public void Open()
    {
        Visible = true;
    }

    public void Close()
    {
        Visible = false;
    }
}
