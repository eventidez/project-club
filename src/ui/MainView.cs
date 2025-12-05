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
        _dateLabel.Text = $"{4}月 {GameSet.Day}日";
    }
}
