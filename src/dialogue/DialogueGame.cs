using GameSystems.RuntimeSet;
using Godot;
using System;
using System.Linq;
using YarnSpinnerGodot;

namespace Game;

public partial class DialogueGame : Node
{
    private static DialogueGame I { get; set; }
    [Export] private DialogueRunner _dialogueRunner = default;
    [Export] private CanvasItem _heartContent = default;
    [Export] private RichTextLabel _heartLabel = default;

    public override void _EnterTree()
    {
        base._EnterTree();
        I = this;
        _heartContent.Visible = false;
    }

    [YarnCommand("Face")]
    public static void Face(string face)
    {
        var roleId = face.Split('_')[0];
        // GD.Print(roleId, " ", face);
        var form = MainGame.Instance.RoleNodes.FindNode<GameRoleForm>((rf) => rf.RoleId == roleId);
        if (form == null)
        {
            return;
        }

        form.SetFace(face);
    }

    [YarnCommand("Heart")]
    public static void Heart(string text)
    {
        I._heartContent.Visible = true;
        I._heartLabel.Text = text;
    }

    [YarnCommand("Fg")]
    public static void Fg(string value)
    {
        var roleId = value.Split('_')[0];
        // GD.Print(roleId, "  ", value);
        // GD.Print(new Godot.Collections.Array<string>(MainGame.Instance.RoleNodes.GetNodes<GameRoleForm>().Select((f)=> f.RoleId)));
        var form = MainGame.Instance.RoleNodes.FindNode<GameRoleForm>((rf) => rf.RoleId.Equals(roleId));
        if (form == null)
        {
            return;
        }

        GD.Print(value);
        form.SetBody(value);
    }
}
