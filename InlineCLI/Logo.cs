using System.Drawing;
using Terminal.Gui.Drawing;
using Terminal.Gui.ViewBase;
using Color = Terminal.Gui.Drawing.Color;
using TgAttribute = Terminal.Gui.Drawing.Attribute;

/// <summary>
///     Renders the Terminal.Gui logo in box-drawing characters with a diagonal gradient.
/// </summary>
public sealed class Logo : View
{
    // @formatter:off
    private const string ART = """
                               ╺┳╸┏━╸┏━┓┏┳┓╻┏┓╻┏━┓╻   ┏━╸╻ ╻╻
                                ┃ ┣╸ ┣┳┛┃┃┃┃┃┗┫┣━┫┃   ┃╺┓┃ ┃┃
                                ╹ ┗━╸╹┗╸╹ ╹╹╹ ╹╹ ╹┗━╸╹┗━┛┗━┛╹
                               """;

    // @formatter:on

    private static readonly string[] _artLines = ART.ReplaceLineEndings ("\n").Split ('\n');

    public Logo ()
    {
        var artWidth = _artLines.Select (line => line.Length).Prepend (0).Max ();

        Width = artWidth;
        Height = _artLines.Length;
    }

    /// <inheritdoc />
    protected override bool OnDrawingContent (DrawContext? context)
    {
        List<Color> stops =
        [
            new (0, 128, 255),
            new (0, 255, 128),
            new (255, 255),
            new (255, 128)
        ];

        List<int> steps = [10];

        Gradient gradient = new (stops, steps);

        var artHeight = 3;
        var artWidth = _artLines [0].Length;

        Dictionary<Point, Color> colorMap =
            gradient.BuildCoordinateColorMapping (artHeight, artWidth, GradientDirection.Diagonal);

        TgAttribute normalAttr = GetAttributeForRole (VisualRole.Normal);

        for (var row = 0; row < _artLines.Length; row++)
        {
            var line = _artLines [row];

            for (var col = 0; col < line.Length; col++)
            {
                var ch = line [col];

                if (ch == ' ')
                {
                    continue;
                }

                if (row < 3)
                {
                    Point coord = new (col, row);

                    if (colorMap.TryGetValue (coord, out Color color))
                    {
                        SetAttribute (new TgAttribute (color, normalAttr.Background));
                    }
                    else
                    {
                        SetAttribute (normalAttr);
                    }
                }
                else
                {
                    SetAttribute (normalAttr);
                }

                Move (col, row);
                AddStr (ch.ToString ());
            }
        }

        return true;
    }
}
