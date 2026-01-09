using Chickensoft.AutoInject;
using Chickensoft.Introspection;
using Godot;
using System;

namespace Game;

[Meta(typeof(IAutoNode))]
public partial class RoleMenuView : Control
{
    public override void _Notification(int what) => this.Notify(what);

    [Export] private Control[] _backgrounds = [];
    [Export] private Godot.Collections.Array<Control> _buttonContainers = [];
    [Dependency] public App App => this.DependOn<App>();

    private int _containerIndex = 0;

    public Action CancelPressed;

    public override void _EnterTree()
    {
        base._EnterTree();
        this.Provide();
    }

    public override void _ExitTree()
    {
        base._ExitTree();
    }

    public override void _Ready()
    {
        base._Ready();
        Visible = false;
    }

    public void Open(IGameRoleForm roleForm = default)
    {
        Visible = true;
        UpdateContainer(0);
    }

    public void Close()
    {
        Visible = false;
    }

    private void Cancel()
    {
        if (_containerIndex == 0)
        {
            CancelPressed?.Invoke();
            return;
        }

        UpdateContainer(_containerIndex - 1);
    }

    private void UpdateContainer(int index)
    {
        _containerIndex = index;
        for (int i = 0; i < _buttonContainers.Count; i++)
        {
            _buttonContainers[i].Visible = i == index;
        }

        for (int i = 0; i < _backgrounds.Length; i++)
        {
            _backgrounds[i].Visible = i == index;
        }
    }

    public void OnChatButtonClicked()
    {
        App.AddDialogue();
        Close();
    }

    public void OnClothingButtonPressed()
    {
        UpdateContainer(1);
    }

    public void OnClothingItemButtonPressed(int index)
    {
        var roleForm = this.FindParent<GameRoleForm>();
        var bodyName = roleForm.RoleId;
        switch (index)
        {
            case 0:
                bodyName = $"{bodyName}_common";
                break;
            case 1:
                bodyName = $"{bodyName}_under";
                break;
            case 2:
                bodyName = $"{bodyName}_sexy";
                break;
        }

        roleForm.SetBody(bodyName);
    }
}
