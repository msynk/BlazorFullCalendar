namespace BlazorFullCalendar;

/// <summary>
/// Provides details about a date range change in the calendar,
/// fired when the user navigates (prev/next/today) or switches views.
/// </summary>
public sealed class BlazorFullCalendarDateChangeEventArgs
{
    /// <summary>Start of the visible date range (inclusive).</summary>
    public required DateTime Start { get; init; }

    /// <summary>End of the visible date range (inclusive).</summary>
    public required DateTime End { get; init; }

    /// <summary>The active calendar view when the change occurred.</summary>
    public required BlazorFullCalendarView View { get; init; }
}
