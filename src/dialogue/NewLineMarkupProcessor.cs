using Godot;
using System;
using System.Collections.Generic;
using System.Text;
using Yarn.Markup;
using YarnSpinnerGodot;

public partial class NewLineMarkupProcessor : ReplacementMarkupHandler
{
    [Export] public LineProviderBehaviour LineProvider;
    private const string NewLineAttributeName = "r";

    public override void _Ready()
    {
        if (!IsInstanceValid(LineProvider))
        {
            GD.PushError($"No {nameof(LineProvider)} is set on this {nameof(NewLineMarkupProcessor)}");
            return;
        }

        LineProvider.RegisterMarkerProcessor(NewLineAttributeName, this);



        LineProvider.RegisterMarkerProcessor("img", this);
    }

    public override List<LineParser.MarkupDiagnostic> ProcessReplacementMarker(MarkupAttribute marker,
        StringBuilder childBuilder, List<MarkupAttribute> childAttributes,
        string localeCode)
    {
        if (marker.Name != NewLineAttributeName)
        {
            return [];
        }

        childBuilder.Insert(0, "\n");
        return NoDiagnostics;
    }
}
