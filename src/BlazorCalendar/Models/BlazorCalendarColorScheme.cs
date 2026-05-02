namespace BlazorCalendar;

/// <summary>
/// Resolved color list and helpers for labels. Built from the calendar's <c>EventColorOptions</c> parameter.
/// </summary>
public sealed class BlazorCalendarColorScheme
{
    private static readonly IReadOnlyList<BlazorCalendarColorOption> DefaultOptions =
        Enum.GetValues<BlazorCalendarEventColor>().Select(c => new BlazorCalendarColorOption { Color = c }).ToArray();

    private readonly Dictionary<BlazorCalendarEventColor, BlazorCalendarColorOption> _byColor;

    public BlazorCalendarColorScheme(IReadOnlyList<BlazorCalendarColorOption>? options)
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

    /// <summary>Configured colors in display order (defaults to every <see cref="BlazorCalendarEventColor"/>).</summary>
    public IReadOnlyList<BlazorCalendarColorOption> Options { get; }

    public BlazorCalendarColorOption? Find(BlazorCalendarEventColor color) =>
        _byColor.TryGetValue(color, out var o) ? o : null;

    /// <summary>
    /// Label for dropdowns, filters, agenda headers, and event details: always the localized color name from
    /// <see cref="BlazorCalendarTexts.GetColorLabel"/>, then <see cref="BlazorCalendarColorOption.Title"/> when set (after an em dash).
    /// </summary>
    public string GetPrimaryLabel(BlazorCalendarEventColor color, BlazorCalendarTexts texts)
    {
        var name = texts.GetColorLabel(color);
        var t = Find(color)?.Title?.Trim();
        return string.IsNullOrEmpty(t) ? name : $"{name} — {t}";
    }

    /// <summary>
    /// Options shown in add/edit dialog. If the event uses a color not present in <see cref="Options"/>, it is appended so the value stays valid.
    /// </summary>
    public IReadOnlyList<BlazorCalendarColorOption> GetEditorOptions(BlazorCalendarEventColor? editingColor)
    {
        if (editingColor is not { } ec || _byColor.ContainsKey(ec))
            return Options;

        var extra = new List<BlazorCalendarColorOption>(Options.Count + 1);
        extra.AddRange(Options);
        extra.Add(new BlazorCalendarColorOption { Color = ec });
        return extra;
    }

    /// <summary>Sort key for agenda grouping: configured order first, then any other enum value.</summary>
    public int GetSortOrder(BlazorCalendarEventColor color)
    {
        for (var i = 0; i < Options.Count; i++)
        {
            if (Options[i].Color == color)
                return i;
        }

        return 1000 + (int)color;
    }
}
