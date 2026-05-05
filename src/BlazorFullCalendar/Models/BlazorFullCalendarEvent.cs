namespace BlazorFullCalendar;

public class BlazorFullCalendarEvent
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public BlazorFullCalendarEventColor Color { get; set; } = BlazorFullCalendarEventColor.Blue;
    public List<BlazorFullCalendarAttendee> Attendees { get; set; } = [];

    public bool IsSingleDay => StartDate.Date == EndDate.Date;
    public bool IsMultiDay => !IsSingleDay;
    public TimeSpan Duration => EndDate - StartDate;
}

