using System.Globalization;
using BlazorCalendar.Models;

namespace BlazorCalendar.Services;

public class CalendarState
{
    private List<CalendarEvent> _allEvents = [];
    private List<CalendarEvent> _filteredEvents = [];

    public DateTime SelectedDate { get; private set; } = DateTime.Today;
    public CalendarView View { get; private set; } = CalendarView.Month;
    public List<EventColor> SelectedColors { get; private set; } = [];

    /// <summary>When set, only events that include this attendee (by <see cref="CalendarHelpers.AttendeeFilterKey"/>) are shown.</summary>
    public string? SelectedAttendeeKey { get; private set; }
    public bool Use24HourFormat { get; private set; } = true;
    public BadgeVariant BadgeVariant { get; private set; } = BadgeVariant.Colored;
    public int StartOfDayHour { get; private set; } = 8;
    public AgendaGroupBy AgendaModeGroupBy { get; private set; } = AgendaGroupBy.Date;
    public bool IsDarkMode { get; private set; }

    public CultureInfo Culture { get; private set; } = CultureInfo.CurrentUICulture;
    public bool IsRtl => Culture.TextInfo.IsRightToLeft;

    // Drag state
    public CalendarEvent? DraggedEvent { get; set; }
    public bool IsDragging => DraggedEvent != null;

    public IReadOnlyList<CalendarEvent> Events => _filteredEvents;
    public IReadOnlyList<CalendarEvent> AllEvents => _allEvents;

    public event Action? OnStateChanged;

    public void Initialize(List<CalendarEvent> events, CultureInfo? culture = null)
    {
        _allEvents = [.. events];
        if (culture != null)
            Culture = culture;
        ApplyFilters();
        NotifyStateChanged();
    }

    public void SetCulture(CultureInfo culture)
    {
        Culture = culture;
        ApplyFilters();
        NotifyStateChanged();
    }

    public void SetSelectedDate(DateTime date)
    {
        SelectedDate = date;
        ApplyFilters();
        NotifyStateChanged();
    }

    public void SetView(CalendarView view)
    {
        View = view;
        ApplyFilters();
        NotifyStateChanged();
    }

    public void SetUse24HourFormat(bool value)
    {
        Use24HourFormat = value;
        NotifyStateChanged();
    }

    public void ToggleTimeFormat()
    {
        Use24HourFormat = !Use24HourFormat;
        NotifyStateChanged();
    }

    public void SetBadgeVariant(BadgeVariant variant)
    {
        BadgeVariant = variant;
        NotifyStateChanged();
    }

    public void SetStartOfDayHour(int hour)
    {
        if (hour >= 0 && hour <= 16)
        {
            StartOfDayHour = hour;
            NotifyStateChanged();
        }
    }

    public void SetAgendaModeGroupBy(AgendaGroupBy groupBy)
    {
        AgendaModeGroupBy = groupBy;
        NotifyStateChanged();
    }

    public void ToggleDarkMode()
    {
        IsDarkMode = !IsDarkMode;
        NotifyStateChanged();
    }

    public void NavigatePrevious()
    {
        SelectedDate = CalendarHelpers.NavigateDate(SelectedDate, View, false, Culture);
        ApplyFilters();
        NotifyStateChanged();
    }

    public void NavigateNext()
    {
        SelectedDate = CalendarHelpers.NavigateDate(SelectedDate, View, true, Culture);
        ApplyFilters();
        NotifyStateChanged();
    }

    public void GoToToday()
    {
        SelectedDate = DateTime.Today;
        ApplyFilters();
        NotifyStateChanged();
    }

    public void AddEvent(CalendarEvent ev)
    {
        _allEvents.Add(ev);
        ApplyFilters();
        NotifyStateChanged();
    }

    public void UpdateEvent(CalendarEvent ev)
    {
        var idx = _allEvents.FindIndex(e => e.Id == ev.Id);
        if (idx >= 0) _allEvents[idx] = ev;
        ApplyFilters();
        NotifyStateChanged();
    }

    public void RemoveEvent(int eventId)
    {
        _allEvents.RemoveAll(e => e.Id == eventId);
        ApplyFilters();
        NotifyStateChanged();
    }

    public void FilterByColor(EventColor color)
    {
        if (SelectedColors.Contains(color))
            SelectedColors.Remove(color);
        else
            SelectedColors.Add(color);
        ApplyFilters();
        NotifyStateChanged();
    }

    public void SetColorFilter(EventColor? color)
    {
        SelectedColors.Clear();
        if (color.HasValue)
            SelectedColors.Add(color.Value);

        ApplyFilters();
        NotifyStateChanged();
    }

    public void SetAttendeeFilter(string? attendeeKey)
    {
        SelectedAttendeeKey = string.IsNullOrWhiteSpace(attendeeKey) ? null : attendeeKey.Trim();
        ApplyFilters();
        NotifyStateChanged();
    }

    /// <summary>Distinct attendees on events visible in the current view/date range.</summary>
    public IReadOnlyList<(string Key, string DisplayName)> GetAttendeesInCurrentView()
    {
        var viewEvents = CalendarHelpers.GetEventsForView(_allEvents.ToList(), View, SelectedDate, Culture);
        var map = new Dictionary<string, string>(StringComparer.Ordinal);
        foreach (var ev in viewEvents)
        {
            foreach (var a in ev.Attendees)
            {
                var key = CalendarHelpers.AttendeeFilterKey(a);
                if (key.Length == 0)
                    continue;
                if (map.ContainsKey(key))
                    continue;
                var label = string.IsNullOrWhiteSpace(a.FullName)
                    ? (string.IsNullOrWhiteSpace(a.Id) ? "(Unnamed)" : a.Id.Trim())
                    : a.FullName.Trim();
                map[key] = label;
            }
        }

        return map
            .OrderBy(kv => kv.Value, StringComparer.CurrentCultureIgnoreCase)
            .Select(kv => (kv.Key, kv.Value))
            .ToList();
    }

    public void ClearFilter()
    {
        SelectedColors.Clear();
        SelectedAttendeeKey = null;
        _filteredEvents = [.. _allEvents];
        NotifyStateChanged();
    }

    private void ApplyFilters()
    {
        PruneInvalidAttendeeFilter();

        var result = _allEvents.AsEnumerable();

        if (SelectedColors.Count > 0)
            result = result.Where(e => SelectedColors.Contains(e.Color));

        if (SelectedAttendeeKey is not null)
            result = result.Where(e => e.Attendees.Any(a => CalendarHelpers.AttendeeFilterKey(a) == SelectedAttendeeKey));

        _filteredEvents = result.ToList();
    }

    private void PruneInvalidAttendeeFilter()
    {
        if (SelectedAttendeeKey is null)
            return;

        var validKeys = CalendarHelpers
            .GetEventsForView(_allEvents.ToList(), View, SelectedDate, Culture)
            .SelectMany(e => e.Attendees)
            .Select(CalendarHelpers.AttendeeFilterKey)
            .Where(k => k.Length > 0)
            .ToHashSet(StringComparer.Ordinal);

        if (!validKeys.Contains(SelectedAttendeeKey))
            SelectedAttendeeKey = null;
    }

    // Drag-and-drop helpers
    public void StartDrag(CalendarEvent ev)
    {
        DraggedEvent = ev;
        NotifyStateChanged();
    }

    public void EndDrag()
    {
        if (DraggedEvent == null)
            return;

        DraggedEvent = null;
        NotifyStateChanged();
    }

    public void HandleDrop(DateTime targetDate, int? hour = null, int? minute = null)
    {
        if (DraggedEvent == null) return;

        var originalStart = DraggedEvent.StartDate;
        var duration = DraggedEvent.Duration;

        var newStart = targetDate.Date;
        if (hour.HasValue)
            newStart = newStart.AddHours(hour.Value).AddMinutes(minute ?? 0);
        else
            newStart = newStart.AddHours(originalStart.Hour).AddMinutes(originalStart.Minute);

        if (newStart == originalStart)
        {
            EndDrag();
            return;
        }

        var updated = new CalendarEvent
        {
            Id = DraggedEvent.Id,
            Title = DraggedEvent.Title,
            Description = DraggedEvent.Description,
            StartDate = newStart,
            EndDate = newStart + duration,
            Color = DraggedEvent.Color,
            Attendees = [.. DraggedEvent.Attendees]
        };

        UpdateEvent(updated);
        EndDrag();
    }

    private void NotifyStateChanged() => OnStateChanged?.Invoke();
}
