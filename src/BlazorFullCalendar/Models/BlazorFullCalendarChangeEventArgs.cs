namespace BlazorFullCalendar;

/// <summary>
/// Identifies the kind of change applied to a calendar event.
/// </summary>
public enum BlazorFullCalendarChangeKind
{
    Add,
    Edit,
    Delete
}

/// <summary>
/// Identifies where a calendar event change originated from in the UI.
/// </summary>
public enum BlazorFullCalendarChangeSource
{
    Dialog,
    Drag,
    Resize,
    Delete
}

/// <summary>
/// Provides details about a user-applied calendar event change.
/// </summary>
public sealed class BlazorFullCalendarChangeEventArgs
{
    /// <summary>
    /// The current event snapshot after the change for Add/Edit,
    /// or the removed event snapshot for Delete.
    /// </summary>
    public required BlazorFullCalendarEvent Event { get; init; }

    /// <summary>
    /// The change type that occurred.
    /// </summary>
    public required BlazorFullCalendarChangeKind Kind { get; init; }

    /// <summary>
    /// The event snapshot before the change for Edit/Delete.
    /// Null for Add.
    /// </summary>
    public BlazorFullCalendarEvent? OldEvent { get; init; }

    /// <summary>
    /// The UI source that triggered this change.
    /// </summary>
    public BlazorFullCalendarChangeSource Source { get; init; }
}

