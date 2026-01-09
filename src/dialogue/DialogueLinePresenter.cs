using Godot;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Yarn.Markup;
using YarnSpinnerGodot;

#nullable enable

namespace Game.Dialogue;

public partial class DialogueLinePresenter : Control, DialoguePresenterBase
{
    [Export] public DialogueRunner? dialogueRunner;
    [Export] public Control? presenterControl;
    [Export] public RichTextLabel? lineText;
    [Export] public bool showCharacterNameInLineView = true;
    [Export] public RichTextLabel? characterNameText;
    [Export] public CanvasItem? characterNameContainer = null;
    [Export] public bool useFadeEffect = true;
    [Export] public float fadeUpDuration = 0.25f;
    [Export] public float fadeDownDuration = 0.1f;
    [Export] public bool autoAdvance = false;
    [Export] public float autoAdvanceDelay = 1f;
    [Export] public bool useTypewriterEffect = true;
    [Export] public int typewriterEffectSpeed = 60;
    [Export] public bool ConvertHTMLToBBCode;

    [Export] Godot.Collections.Array<ActionMarkupHandler> eventHandlers = [];

    public const string HtmlTagPattern = @"<(.*?)>";

    public YarnTask OnDialogueCompleteAsync()
    {
        if (IsInstanceValid(presenterControl))
        {
            presenterControl!.Visible = false;
        }

        return YarnTask.CompletedTask;
    }

    public List<IActionMarkupHandler> ActionMarkupHandlers { get; } = [];

    /// <inheritdoc/>
    public YarnTask OnDialogueStartedAsync()
    {
        return YarnTask.CompletedTask;
    }


    public override void _Ready()
    {
        if (IsInstanceValid(presenterControl))
        {
            presenterControl!.Visible = false;
        }

        if (useTypewriterEffect)
        {
            // need to add a pause handler also
            // and add it to the front of the list
            // that way it always happens first
            var pauser = new PauseEventProcessor();
            ActionMarkupHandlers.Insert(0, pauser);
        }

        if (IsInstanceValid(lineText))
        {
            lineText.VisibleCharactersBehavior = TextServer.VisibleCharactersBehavior.CharsAfterShaping;
        }

        if (characterNameContainer == null && characterNameText != null)
        {
            characterNameContainer = characterNameText;
        }

        // we add all the Godot Node-derived handlers into the shared list
        ActionMarkupHandlers.AddRange(eventHandlers);
    }

    /// <summary>Presents a line using the configured text view.</summary>
    /// <inheritdoc cref="DialoguePresenterBase.RunLineAsync(LocalizedLine, LineCancellationToken)" path="/param"/>
    /// <inheritdoc cref="DialoguePresenterBase.RunLineAsync(LocalizedLine, LineCancellationToken)" path="/returns"/>
    public async YarnTask RunLineAsync(LocalizedLine line, LineCancellationToken token)
    {
        if (lineText == null)
        {
            GD.PushError($"Line Presenter does not have a text view. Skipping line {line.TextID} (\"{line.RawText}\")");
            return;
        }

        MarkupParseResult text;

        // configuring the text fields
        if (showCharacterNameInLineView)
        {
            if (characterNameText == null)
            {
                GD.PushWarning(
                    $"{nameof(LinePresenter)} is configured to show character names, but no character name text view was provided.",
                    this);
            }
            else
            {
                characterNameText.Text = line.CharacterName;
            }

            text = line.TextWithoutCharacterName;

            if (line.Text.TryGetAttributeWithName("character", out var characterAttribute))
            {
                text.Attributes.Add(characterAttribute);
            }
        }
        else
        {
            // we don't want to show character names but do have a valid container for showing them
            // so we should just disable that and continue as if it didn't exist
            if (IsInstanceValid(characterNameContainer))
            {
                characterNameContainer!.Visible = false;
            }

            text = line.TextWithoutCharacterName;
        }

        lineText.Text = text.Text.Replace("\\\\n", "\n");

        lineText.VisibleRatio = 0;
        // letting every action markup handler know that fade up (if set) is about to begin
        foreach (var processor in ActionMarkupHandlers)
        {
            processor.OnPrepareForLine(text, lineText);
        }

        if (IsInstanceValid(presenterControl))
        {
            // fading up the UI
            presenterControl!.Visible = true;
            if (useFadeEffect)
            {
                await Effects.FadeAlphaAsync(presenterControl, 0, 1, fadeUpDuration, token.HurryUpToken);
                if (!IsInstanceValid(this))
                {
                    return;
                }
            }
            else
            {
                // We're not fading up, so set the presenter control's alpha to 1 immediately.
                var oldModulate = presenterControl.Modulate;
                presenterControl.Modulate = new Color(oldModulate.R, oldModulate.G, oldModulate.B, 1.0f);
            }
        }

        if (useTypewriterEffect)
        {
            var typewriter = new BasicTypewriter()
            {
                ActionMarkupHandlers = this.ActionMarkupHandlers,
                Text = this.lineText,
                CharactersPerSecond = this.typewriterEffectSpeed,
                ConvertHTMLToBBCode = this.ConvertHTMLToBBCode,
            };

            await typewriter.RunTypewriter(text, token.HurryUpToken);
            if (!IsInstanceValid(this))
            {
                return;
            }
        }

        // if we are set to autoadvance how long do we hold for before continuing?
        if (autoAdvance)
        {
            await YarnTask.Delay((int)(autoAdvanceDelay * 1000), token.NextLineToken).SuppressCancellationThrow();
            if (!IsInstanceValid(this))
            {
                return;
            }
        }
        else
        {
            await YarnTask.WaitUntilCanceled(token.NextLineToken).SuppressCancellationThrow();
            if (!IsInstanceValid(this))
            {
                return;
            }
        }

        // we tell all action processors that the line is finished and is about to go away
        foreach (var processor in ActionMarkupHandlers)
        {
            processor.OnLineWillDismiss();
        }

        if (IsInstanceValid(presenterControl))
        {
            // we fade down the UI
            if (useFadeEffect)
            {
                await Effects.FadeAlphaAsync(presenterControl, 1, 0, fadeDownDuration, token.HurryUpToken);
                if (!IsInstanceValid(this))
                {
                    return;
                }
            }

            presenterControl!.Visible = false;
        }
    }

    /// <inheritdoc cref="DialoguePresenterBase.RunOptionsAsync(DialogueOption[], CancellationToken)" path="/summary"/> 
    /// <inheritdoc cref="DialoguePresenterBase.RunOptionsAsync(DialogueOption[], CancellationToken)" path="/param"/> 
    /// <inheritdoc cref="DialoguePresenterBase.RunOptionsAsync(DialogueOption[], CancellationToken)" path="/returns"/> 
    /// <remarks>
    /// This Dialogue Presenter does not handle any options.
    /// </remarks>
    public YarnTask<DialogueOption?> RunOptionsAsync(DialogueOption[] dialogueOptions,
        System.Threading.CancellationToken cancellationToken)
    {
        return YarnTask<DialogueOption?>.FromResult(null);
    }

    private void OnContinuePressed()
    {
        if (!IsInstanceValid(dialogueRunner))
        {
            GD.PushWarning($"Continue button was clicked, but {nameof(dialogueRunner)} is invalid!", this);
            return;
        }

        dialogueRunner!.RequestNextLine();
    }

    private void ConvertHTMLToBBCodeIfConfigured()
    {
        if (ConvertHTMLToBBCode)
        {
            if (IsInstanceValid(characterNameText))
            {
                characterNameText!.Text = Regex.Replace(characterNameText.Text, HtmlTagPattern, "[$1]");
            }

            lineText!.Text = Regex.Replace(lineText.Text, HtmlTagPattern, "[$1]");
        }
    }
}
