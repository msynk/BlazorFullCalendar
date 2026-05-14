namespace BlazorFullCalendar;

public class BlazorFullCalendarEvent
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public BlazorFullCalendarEventColor Color { get; set; } = BlazorFullCalendarEventColor.Blue;
    public List<BlazorFullCalendarAttendee> Attendees { get; set; } = [];

    public bool IsSingleDay => StartDate.Date == EndDate.Date;
    public bool IsMultiDay => !IsSingleDay;
    public TimeSpan Duration => EndDate - StartDate;

    public object? Data { get; set; }
}

