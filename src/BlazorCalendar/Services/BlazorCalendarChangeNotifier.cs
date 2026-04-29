using BlazorCalendar.Models;

namespace BlazorCalendar.Services;

/// <summary>
/// Dispatches calendar event change notifications to the component consumer.
/// Also provides wrappers for mutation paths that need pre/post snapshots.
/// </summary>
public sealed class BlazorCalendarChangeNotifier
{
    private readonly CalendarState _state;
    private readonly Func<BlazorCalendarChangeEventArgs, Task> _dispatch;

    public BlazorCalendarChangeNotifier(CalendarState state, Func<BlazorCalendarChangeEventArgs, Task> dispatch)
    {
        _state = state;
        _dispatch = dispatch;
    }

    /// <summary>
    /// Dispatches a change payload to the component's <c>OnChange</c> callback.
    /// </summary>
    public Task NotifyAsync(BlazorCalendarChangeEventArgs args) => _dispatch(args);

    /// <summary>
    /// Applies drop logic through <see cref="CalendarState.HandleDrop"/> and emits
    /// an Edit change when the event date-time has actually changed.
    /// </summary>
    public Task HandleDropAsync(DateTime targetDate, int? hour = null, int? minute = null)
    {
        var dragged = _state.DraggedEvent;
        if (dragged is null)
            return Task.CompletedTask;

        var oldSnapshot = CloneEvent(dragged);
        var eventId = dragged.Id;

        _state.HandleDrop(targetDate, hour, minute);

        var after = _state.AllEvents.FirstOrDefault(e => e.Id == eventId);
        if (after is null)
            return Task.CompletedTask;

        if (after.StartDate == oldSnapshot.StartDate && after.EndDate == oldSnapshot.EndDate)
            return Task.CompletedTask;

        return NotifyAsync(new BlazorCalendarChangeEventArgs
        {
            Event = CloneEvent(after),
            OldEvent = oldSnapshot,
            Kind = CalendarChangeKind.Edit,
            Source = CalendarChangeSource.Drag
        });
    }

    /// <summary>
    /// Creates a deep snapshot of a calendar event payload suitable for change args.
    /// </summary>
    public static CalendarEvent CloneEvent(CalendarEvent source) =>
        new()
        {
            Id = source.Id,
            Title = source.Title,
            Description = source.Description,
            StartDate = source.StartDate,
            EndDate = source.EndDate,
            Color = source.Color,
            Attendees = source.Attendees
                .Select(a => new CalendarAttendee
                {
                    FirstName = a.FirstName,
                    LastName = a.LastName,
                    Id = a.Id
                })
                .ToList()
        };
}
