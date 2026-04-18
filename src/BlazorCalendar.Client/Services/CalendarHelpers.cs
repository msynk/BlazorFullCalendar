using System.Globalization;
using BlazorCalendar.Client.Models;

namespace BlazorCalendar.Client.Services;

public static class CalendarHelpers
{
    public const int HourHeightPx = 96;
    private const string FormatString = "MMM d, yyyy";

    public static string RangeText(CalendarView view, DateTime date)
    {
        DateTime start, end;
        switch (view)
        {
            case CalendarView.Month:
                start = new DateTime(date.Year, date.Month, 1);
                end = start.AddMonths(1).AddDays(-1);
                break;
            case CalendarView.Week:
                start = StartOfWeek(date);
                end = start.AddDays(6);
                break;
            case CalendarView.Day:
                return date.ToString(FormatString, CultureInfo.InvariantCulture);
            case CalendarView.Year:
                start = new DateTime(date.Year, 1, 1);
                end = new DateTime(date.Year, 12, 31);
                break;
            case CalendarView.Agenda:
                start = new DateTime(date.Year, date.Month, 1);
                end = start.AddMonths(1).AddDays(-1);
                break;
            default:
                return "Error";
        }

        return $"{start.ToString(FormatString, CultureInfo.InvariantCulture)} - {end.ToString(FormatString, CultureInfo.InvariantCulture)}";
    }

    public static DateTime NavigateDate(DateTime date, CalendarView view, bool forward)
    {
        return view switch
        {
            CalendarView.Month => date.AddMonths(forward ? 1 : -1),
            CalendarView.Week => date.AddDays(forward ? 7 : -7),
            CalendarView.Day => date.AddDays(forward ? 1 : -1),
            CalendarView.Year => date.AddYears(forward ? 1 : -1),
            CalendarView.Agenda => date.AddMonths(forward ? 1 : -1),
            _ => date
        };
    }

    public static DateTime StartOfWeek(DateTime date, DayOfWeek startDay = DayOfWeek.Sunday)
    {
        int diff = (7 + (date.DayOfWeek - startDay)) % 7;
        return date.Date.AddDays(-diff);
    }

    public static List<CalendarCell> GetCalendarCells(DateTime selectedDate)
    {
        var year = selectedDate.Year;
        var month = selectedDate.Month;
        var firstDay = new DateTime(year, month, 1);
        var daysInMonth = DateTime.DaysInMonth(year, month);
        int firstDayOfWeek = (int)firstDay.DayOfWeek;

        var prevMonth = firstDay.AddMonths(-1);
        int daysInPrevMonth = DateTime.DaysInMonth(prevMonth.Year, prevMonth.Month);

        var cells = new List<CalendarCell>();

        for (int i = 0; i < firstDayOfWeek; i++)
        {
            int day = daysInPrevMonth - firstDayOfWeek + i + 1;
            cells.Add(new CalendarCell
            {
                Day = day,
                CurrentMonth = false,
                Date = new DateTime(prevMonth.Year, prevMonth.Month, day)
            });
        }

        for (int i = 1; i <= daysInMonth; i++)
        {
            cells.Add(new CalendarCell
            {
                Day = i,
                CurrentMonth = true,
                Date = new DateTime(year, month, i)
            });
        }

        int totalDays = firstDayOfWeek + daysInMonth;
        int remaining = (7 - (totalDays % 7)) % 7;
        var nextMonth = firstDay.AddMonths(1);
        for (int i = 1; i <= remaining; i++)
        {
            cells.Add(new CalendarCell
            {
                Day = i,
                CurrentMonth = false,
                Date = new DateTime(nextMonth.Year, nextMonth.Month, i)
            });
        }

        return cells;
    }

    public static string FormatTime(DateTime date, bool use24Hour)
    {
        return use24Hour
            ? date.ToString("HH:mm", CultureInfo.InvariantCulture)
            : date.ToString("h:mm tt", CultureInfo.InvariantCulture);
    }

    public static string FormatHourLabel(int hour, bool use24Hour)
    {
        var dt = DateTime.Today.AddHours(hour);
        return use24Hour
            ? dt.ToString("HH:00", CultureInfo.InvariantCulture)
            : dt.ToString("h tt", CultureInfo.InvariantCulture);
    }

    public static List<List<CalendarEvent>> GroupEvents(List<CalendarEvent> dayEvents)
    {
        var sorted = dayEvents.OrderBy(e => e.StartDate).ToList();
        var groups = new List<List<CalendarEvent>>();

        foreach (var ev in sorted)
        {
            bool placed = false;
            foreach (var group in groups)
            {
                if (ev.StartDate >= group[^1].EndDate)
                {
                    group.Add(ev);
                    placed = true;
                    break;
                }
            }
            if (!placed)
                groups.Add([ev]);
        }

        return groups;
    }

    public static (double TopPercent, double WidthPercent, double LeftPercent) GetEventBlockStyle(
        CalendarEvent ev, DateTime day, int groupIndex, int groupSize)
    {
        var dayStart = day.Date;
        var eventStart = ev.StartDate < dayStart ? dayStart : ev.StartDate;
        double startMinutes = (eventStart - dayStart).TotalMinutes;
        double top = (startMinutes / 1440.0) * 100;
        double width = 100.0 / groupSize;
        double left = groupIndex * width;
        return (top, width, left);
    }

    public static Dictionary<int, int> CalculateMonthEventPositions(
        List<CalendarEvent> multiDayEvents,
        List<CalendarEvent> singleDayEvents,
        DateTime selectedDate)
    {
        var monthStart = new DateTime(selectedDate.Year, selectedDate.Month, 1);
        var monthEnd = monthStart.AddMonths(1).AddDays(-1);

        var eventPositions = new Dictionary<int, int>();
        var occupiedPositions = new Dictionary<string, bool[]>();

        for (var d = monthStart; d <= monthEnd; d = d.AddDays(1))
            occupiedPositions[d.ToString("O")] = new bool[3];

        var sorted = multiDayEvents
            .OrderByDescending(e => (e.EndDate - e.StartDate).TotalDays)
            .ThenBy(e => e.StartDate)
            .Concat(singleDayEvents.OrderBy(e => e.StartDate))
            .ToList();

        foreach (var ev in sorted)
        {
            var evStart = ev.StartDate.Date;
            var evEnd = ev.EndDate.Date;
            var rangeStart = evStart < monthStart ? monthStart : evStart;
            var rangeEnd = evEnd > monthEnd ? monthEnd : evEnd;

            var eventDays = new List<DateTime>();
            for (var d = rangeStart; d <= rangeEnd; d = d.AddDays(1))
                eventDays.Add(d);

            int position = -1;
            for (int i = 0; i < 3; i++)
            {
                if (eventDays.All(d =>
                {
                    var key = d.ToString("O");
                    return occupiedPositions.ContainsKey(key) && !occupiedPositions[key][i];
                }))
                {
                    position = i;
                    break;
                }
            }

            if (position != -1)
            {
                foreach (var d in eventDays)
                {
                    var key = d.ToString("O");
                    if (occupiedPositions.TryGetValue(key, out var slots))
                        slots[position] = true;
                }
                eventPositions[ev.Id] = position;
            }
        }

        return eventPositions;
    }

    public static List<(CalendarEvent Event, int Position, bool IsMultiDay)> GetMonthCellEvents(
        DateTime date, List<CalendarEvent> events, Dictionary<int, int> eventPositions)
    {
        var dayStart = date.Date;
        var eventsForDate = events.Where(ev =>
        {
            var s = ev.StartDate.Date;
            var e = ev.EndDate.Date;
            return (dayStart >= s && dayStart <= e) || s == dayStart || e == dayStart;
        }).ToList();

        return eventsForDate
            .Select(ev => (
                Event: ev,
                Position: eventPositions.GetValueOrDefault(ev.Id, -1),
                IsMultiDay: ev.IsMultiDay
            ))
            .OrderByDescending(x => x.IsMultiDay)
            .ThenBy(x => x.Position)
            .ToList();
    }

    public static List<CalendarEvent> GetEventsForDay(List<CalendarEvent> events, DateTime date, bool weekOnly = false)
    {
        var target = date.Date;
        return events.Where(ev =>
        {
            var s = ev.StartDate.Date;
            var e = ev.EndDate.Date;
            if (weekOnly)
                return ev.IsMultiDay && s <= target && e >= target;
            return s <= target && e >= target;
        }).ToList();
    }

    public static List<CalendarEvent> GetEventsForWeek(List<CalendarEvent> events, DateTime date)
    {
        var weekStart = StartOfWeek(date, DayOfWeek.Monday);
        var weekEnd = weekStart.AddDays(6);
        return events.Where(ev =>
            ev.StartDate.Date <= weekEnd && ev.EndDate.Date >= weekStart).ToList();
    }

    public static List<CalendarEvent> GetEventsForMonth(List<CalendarEvent> events, DateTime date)
    {
        var monthStart = new DateTime(date.Year, date.Month, 1);
        var monthEnd = monthStart.AddMonths(1).AddDays(-1);
        return events.Where(ev =>
            ev.StartDate.Date <= monthEnd && ev.EndDate.Date >= monthStart).ToList();
    }

    public static List<CalendarEvent> GetEventsForYear(List<CalendarEvent> events, DateTime date)
    {
        var yearStart = new DateTime(date.Year, 1, 1);
        var yearEnd = new DateTime(date.Year, 12, 31);
        return events.Where(ev =>
            ev.StartDate.Date <= yearEnd && ev.EndDate.Date >= yearStart).ToList();
    }

    public static DateTime[] GetWeekDates(DateTime date, DayOfWeek startDay = DayOfWeek.Sunday)
    {
        var start = StartOfWeek(date, startDay);
        return Enumerable.Range(0, 7).Select(i => start.AddDays(i)).ToArray();
    }

    public static string GetColorCss(EventColor color) => color switch
    {
        EventColor.Blue => "cal-color-blue",
        EventColor.Green => "cal-color-green",
        EventColor.Red => "cal-color-red",
        EventColor.Yellow => "cal-color-yellow",
        EventColor.Purple => "cal-color-purple",
        EventColor.Orange => "cal-color-orange",
        _ => "cal-color-blue"
    };

    public static string GetBgColorCss(EventColor color) => color switch
    {
        EventColor.Blue => "cal-bg-blue",
        EventColor.Green => "cal-bg-green",
        EventColor.Red => "cal-bg-red",
        EventColor.Yellow => "cal-bg-yellow",
        EventColor.Purple => "cal-bg-purple",
        EventColor.Orange => "cal-bg-orange",
        _ => "cal-bg-blue"
    };

    public static double GetCurrentTimePosition()
    {
        var now = DateTime.Now;
        double minutes = now.Hour * 60 + now.Minute;
        return (minutes / 1440.0) * 100;
    }

    public static string Capitalize(string str)
    {
        if (string.IsNullOrEmpty(str)) return "";
        return char.ToUpperInvariant(str[0]) + str[1..];
    }
}
