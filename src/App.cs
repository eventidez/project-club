using Godot;
using System;
using Chickensoft.AutoInject;
using Chickensoft.Introspection;
using GameSystems.Setting;

namespace Game;

[Meta(typeof(IAutoNode))]
public partial class App : Node, IProvide<SettingFile>
{
    public override void _Notification(int what) => this.Notify(what);

    [Export] private SettingFile _settingFile = default;
    [Export] private Godot.Collections.Dictionary _dict = default;
    private TestX _test = default;

    SettingFile IProvide<SettingFile>.Value() => _settingFile;

    public override void _EnterTree()
    {
        base._EnterTree();

        var json = ResourceLoader.Load<Json>($"res://configs/settings/global.json");
        json.ToResources<DSetting>((d) =>
        {
            _settingFile.SetValue("global", d.Id, d.InitialValue);

        });

        json = ResourceLoader.Load<Json>($"res://configs/settings/settings.json");
        json.ToResources<DSetting>((d) =>
        {
            _settingFile.SetValue("settings", d.Id, d.InitialValue);

        });


        this.Provide();
        // GD.PrintErr(GetPath(), "  _EnterTree");
    }

    public override void _Ready()
    {
        base._Ready();
        // GD.PrintErr(GetPath(), "  _Ready");
        // foreach (var groupName in _settingFile.GetGroups())
        // {
        //     GD.Print(Json.Stringify(_settingFile.GetSettings(groupName), "\t"));
        // }
    }

    public void AddGame()
    {
        if (_test == null)
        {
            _test = new TestX();
        }

        if (_test.GetParent() == null)
        {
            AddChild(_test);
        }

        // GD.PrintErr(GetPath(), "  AddGame");
    }

    public void RemoveGame()
    {
        if (_test == null)
        {
            return;
        }

        if (_test.GetParent() == this)
        {
            RemoveChild(_test);
        }
        // AddChild(new Game());
        // GD.PrintErr(GetPath(), "  AddGame");
    }
}
