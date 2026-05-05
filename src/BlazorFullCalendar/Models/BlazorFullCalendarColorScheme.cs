namespace BlazorFullCalendar;

/// <summary>
/// Resolved color list and helpers for labels. Built from the calendar's <c>EventColorOptions</c> parameter.
/// </summary>
public sealed class BlazorFullCalendarColorScheme
{
    private static readonly IReadOnlyList<BlazorFullCalendarColorOption> DefaultOptions =
        Enum.GetValues<BlazorFullCalendarEventColor>().Select(c => new BlazorFullCalendarColorOption { Color = c }).ToArray();

    private readonly Dictionary<BlazorFullCalendarEventColor, BlazorFullCalendarColorOption> _byColor;

    public BlazorFullCalendarColorScheme(IReadOnlyList<BlazorFullCalendarColorOption>? options)
    {
        var list = options is { Count: > 0 } ? options : DefaultOptions;
        Options = list;
        _byColor = [];
        foreach (var o in list)
        {
            if (!_byColor.ContainsKey(o.Color))
                _byColor[o.Color] = o;
        }
    }

    /// <summary>Configured colors in display order (defaults to every <see cref="BlazorFullCalendarEventColor"/>).</summary>
    public IReadOnlyList<BlazorFullCalendarColorOption> Options { get; }

    public BlazorFullCalendarColorOption? Find(BlazorFullCalendarEventColor color) =>
        _byColor.TryGetValue(color, out var o) ? o : null;

    /// <summary>
    /// Label for dropdowns, filters, agenda headers, and event details: always the localized color name from
    /// <see cref="BlazorFullCalendarTexts.GetColorLabel"/>, then <see cref="BlazorFullCalendarColorOption.Title"/> when set (after an em dash).
    /// </summary>
    public string GetPrimaryLabel(BlazorFullCalendarEventColor color, BlazorFullCalendarTexts texts)
    {
        var name = texts.GetColorLabel(color);
        var t = Find(color)?.Title?.Trim();
        return string.IsNullOrEmpty(t) ? name : $"{name} â€” {t}";
    }

    /// <summary>
    /// Options shown in add/edit dialog. If the event uses a color not present in <see cref="Options"/>, it is appended so the value stays valid.
    /// </summary>
    public IReadOnlyList<BlazorFullCalendarColorOption> GetEditorOptions(BlazorFullCalendarEventColor? editingColor)
    {
        if (editingColor is not { } ec || _byColor.ContainsKey(ec))
            return Options;

        var extra = new List<BlazorFullCalendarColorOption>(Options.Count + 1);
        extra.AddRange(Options);
        extra.Add(new BlazorFullCalendarColorOption { Color = ec });
        return extra;
    }

    /// <summary>Sort key for agenda grouping: configured order first, then any other enum value.</summary>
    public int GetSortOrder(BlazorFullCalendarEventColor color)
    {
        for (var i = 0; i < Options.Count; i++)
        {
            if (Options[i].Color == color)
                return i;
        }

        return 1000 + (int)color;
    }
}

