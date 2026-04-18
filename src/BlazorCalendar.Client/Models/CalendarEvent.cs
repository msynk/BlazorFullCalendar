namespace BlazorCalendar.Client.Models;

public class CalendarEvent
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public EventColor Color { get; set; } = EventColor.Blue;
    public CalendarUser User { get; set; } = new();

    public bool IsSingleDay => StartDate.Date == EndDate.Date;
    public bool IsMultiDay => !IsSingleDay;
    public TimeSpan Duration => EndDate - StartDate;
}
