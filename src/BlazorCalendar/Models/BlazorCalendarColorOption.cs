namespace BlazorCalendar;

/// <summary>
/// Describes one selectable event color in the calendar UI (picker, filters, agenda headers).
/// The list and order are controlled by the calendar component's <c>EventColorOptions</c> parameter.
/// </summary>
public sealed class BlazorCalendarColorOption
{
    /// <summary>Palette slot; maps to built-in CSS classes (e.g. <c>cal-color-blue</c>).</summary>
    public BlazorCalendarEventColor Color { get; set; }

    /// <summary>
    /// Optional text appended after the localized color name (e.g. room or track), separated by an em dash.
    /// When null or whitespace, only <see cref="BlazorCalendarTexts.GetColorLabel"/> for <see cref="Color"/> is shown.
    /// </summary>
    public string? Title { get; set; }
}
