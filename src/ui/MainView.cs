using Godot;
using System;
using Chickensoft.AutoInject;
using Chickensoft.Introspection;

namespace Game.UI;

[Meta(typeof(IAutoNode))]
public partial class MainView : BaseView
{
    [Signal] public delegate void NextDayButtonPressedEventHandler();

    public override void _Notification(int what) => this.Notify(what);

    [Export] private Label _dateLabel = default;
    [Export] private Godot.Collections.Array<Label> _monthLabels = [];
    [Export] private Godot.Collections.Array<Label> _dayLabels = [];
    [Export] private BaseButton _nextDayButton = default;

    [Dependency] public IMainGameRepo MainGameRepo => this.DependOn<IMainGameRepo>();
    [Dependency] public MainGameSet GameSet => this.DependOn<MainGameSet>();

    public override void _EnterTree()
    {
        base._EnterTree();

        // GD.Print(nameof(_EnterTree));
    }

    public override void _ExitTree()
    {
        base._ExitTree();
    }

    public void OnResolved()
    {
        GameSet.ValueChanged += DrawContent;
    }

    public void OnExitTree()
    {
        GameSet.ValueChanged -= DrawContent;
    }

    public override void _Draw()
    {
        base._Draw();
        DrawContent();
    }

    public void NextDay()
    {
        // GameSet.Day += 1;   // ...
        // EmitSignal(SignalName.NextDayButtonPressed);
        MainGameRepo.NextDay();
    }

    private void DrawContent()
    {
        var month = 4.ToString($"D{_monthLabels.Count}");
        for (int i = 0; i < _monthLabels.Count; i++)
        {
            _monthLabels[i].Text = month[i].ToString();
        }

        var day = GameSet.Day.ToString($"D{_dayLabels.Count}");
        for (int i = 0; i < _dayLabels.Count; i++)
        {
            _dayLabels[i].Text = day[i].ToString();
        }

        // _dateLabel.Text = $"{4}月 {GameSet.Day}日";
    }
}
