using BlazorCalendar.Models;

namespace BlazorCalendar.Services;

public class CalendarState
{
    private List<CalendarEvent> _allEvents = [];
    private List<CalendarEvent> _filteredEvents = [];

    public DateTime SelectedDate { get; private set; } = DateTime.Today;
    public CalendarView View { get; private set; } = CalendarView.Month;
    public List<CalendarUser> Users { get; private set; } = [];
    public string SelectedUserId { get; private set; } = "all";
    public List<EventColor> SelectedColors { get; private set; } = [];
    public bool Use24HourFormat { get; private set; } = true;
    public BadgeVariant BadgeVariant { get; private set; } = BadgeVariant.Colored;
    public int StartOfDayHour { get; private set; } = 8;
    public AgendaGroupBy AgendaModeGroupBy { get; private set; } = AgendaGroupBy.Date;
    public bool IsDarkMode { get; private set; }

    // Drag state
    public CalendarEvent? DraggedEvent { get; set; }
    public bool IsDragging => DraggedEvent != null;

    public IReadOnlyList<CalendarEvent> Events => _filteredEvents;
    public IReadOnlyList<CalendarEvent> AllEvents => _allEvents;

    public event Action? OnStateChanged;

    public void Initialize(List<CalendarEvent> events, List<CalendarUser> users)
    {
        _allEvents = [.. events];
        _filteredEvents = [.. events];
        Users = users;
        NotifyStateChanged();
    }

    public void SetSelectedDate(DateTime date)
    {
        SelectedDate = date;
        NotifyStateChanged();
    }

    public void SetView(CalendarView view)
    {
        View = view;
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
        SelectedDate = CalendarHelpers.NavigateDate(SelectedDate, View, false);
        NotifyStateChanged();
    }

    public void NavigateNext()
    {
        SelectedDate = CalendarHelpers.NavigateDate(SelectedDate, View, true);
        NotifyStateChanged();
    }

    public void GoToToday()
    {
        SelectedDate = DateTime.Today;
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

    public void FilterByUser(string userId)
    {
        SelectedUserId = userId;
        ApplyFilters();
        NotifyStateChanged();
    }

    public void ClearFilter()
    {
        SelectedColors.Clear();
        SelectedUserId = "all";
        _filteredEvents = [.. _allEvents];
        NotifyStateChanged();
    }

    private void ApplyFilters()
    {
        var result = _allEvents.AsEnumerable();

        if (SelectedColors.Count > 0)
            result = result.Where(e => SelectedColors.Contains(e.Color));

        if (SelectedUserId != "all")
            result = result.Where(e => e.User.Id == SelectedUserId);

        _filteredEvents = result.ToList();
    }

    // Drag-and-drop helpers
    public void StartDrag(CalendarEvent ev) => DraggedEvent = ev;

    public void EndDrag() => DraggedEvent = null;

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
            User = DraggedEvent.User
        };

        UpdateEvent(updated);
        EndDrag();
    }

    private void NotifyStateChanged() => OnStateChanged?.Invoke();
}
