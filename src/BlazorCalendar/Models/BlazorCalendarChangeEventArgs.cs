namespace BlazorCalendar;

/// <summary>
/// Identifies the kind of change applied to a calendar event.
/// </summary>
public enum BlazorCalendarChangeKind
{
    Add,
    Edit,
    Delete
}

/// <summary>
/// Identifies where a calendar event change originated from in the UI.
/// </summary>
public enum BlazorCalendarChangeSource
{
    Dialog,
    Drag,
    Resize,
    Delete
}

/// <summary>
/// Provides details about a user-applied calendar event change.
/// </summary>
public sealed class BlazorCalendarChangeEventArgs
{
    /// <summary>
    /// The current event snapshot after the change for Add/Edit,
    /// or the removed event snapshot for Delete.
    /// </summary>
    public required BlazorCalendarEvent Event { get; init; }

    /// <summary>
    /// The change type that occurred.
    /// </summary>
    public required BlazorCalendarChangeKind Kind { get; init; }

    /// <summary>
    /// The event snapshot before the change for Edit/Delete.
    /// Null for Add.
    /// </summary>
    public BlazorCalendarEvent? OldEvent { get; init; }

    /// <summary>
    /// The UI source that triggered this change.
    /// </summary>
    public BlazorCalendarChangeSource Source { get; init; }
}
