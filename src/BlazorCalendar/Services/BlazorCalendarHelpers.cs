using System.Globalization;
using BlazorCalendar.Models;

namespace BlazorCalendar.Services;

public static class BlazorCalendarHelpers
{
    public const int HourHeightPx = 96;
    private const string FormatString = "MMM d, yyyy";

    // ── Culture-aware: Range text ────────────────────────────────────────────

    public static string RangeText(BlazorCalendarView view, DateTime date, CultureInfo? culture = null)
    {
        culture ??= CultureInfo.CurrentUICulture;
        var cal = culture.Calendar;
        var dtf = culture.DateTimeFormat;

        switch (view)
        {
            case BlazorCalendarView.Month:
            case BlazorCalendarView.Agenda:
            {
                int y = cal.GetYear(date);
                int m = cal.GetMonth(date);
                string monthName = dtf.GetMonthName(m);
                return $"{monthName} {y}";
            }
            case BlazorCalendarView.Week:
            {
                var start = StartOfWeek(date, culture);
                var end = start.AddDays(6);
                return $"{FormatCultureDate(start, culture)} – {FormatCultureDate(end, culture)}";
            }
            case BlazorCalendarView.Day:
                return FormatCultureDate(date, culture);
            case BlazorCalendarView.Year:
            {
                int y = cal.GetYear(date);
                return y.ToString(culture);
            }
            default:
                return "Error";
        }
    }

    /// <summary>Formats a date as "Mon d, Year" using the supplied culture's calendar.</summary>
    public static string FormatCultureDate(DateTime date, CultureInfo? culture = null)
    {
        culture ??= CultureInfo.CurrentUICulture;
        var cal = culture.Calendar;
        var dtf = culture.DateTimeFormat;
        int y = cal.GetYear(date);
        int m = cal.GetMonth(date);
        int d = cal.GetDayOfMonth(date);
        string abbr = dtf.GetAbbreviatedMonthName(m);
        return $"{abbr} {d}, {y}";
    }

    // ── Culture-aware: Navigation ────────────────────────────────────────────

    public static DateTime NavigateDate(DateTime date, BlazorCalendarView view, bool forward, CultureInfo? culture = null)
    {
        culture ??= CultureInfo.CurrentUICulture;
        var cal = culture.Calendar;
        int delta = forward ? 1 : -1;
        return view switch
        {
            BlazorCalendarView.Month  => cal.AddMonths(date, delta),
            BlazorCalendarView.Week   => date.AddDays(forward ? 7 : -7),
            BlazorCalendarView.Day    => date.AddDays(delta),
            BlazorCalendarView.Year   => cal.AddYears(date, delta),
            BlazorCalendarView.Agenda => cal.AddMonths(date, delta),
            _                   => date
        };
    }

    // ── Culture-aware: Week helpers ──────────────────────────────────────────

    public static DateTime StartOfWeek(DateTime date, CultureInfo? culture = null)
    {
        var startDay = culture?.DateTimeFormat.FirstDayOfWeek ?? DayOfWeek.Sunday;
        return StartOfWeek(date, startDay);
    }

    public static DateTime StartOfWeek(DateTime date, DayOfWeek startDay)
    {
        int diff = (7 + (date.DayOfWeek - startDay)) % 7;
        return date.Date.AddDays(-diff);
    }

    public static DateTime[] GetWeekDates(DateTime date, CultureInfo? culture = null)
    {
        var start = StartOfWeek(date, culture);
        return Enumerable.Range(0, 7).Select(i => start.AddDays(i)).ToArray();
    }

    // ── Culture-aware: Weekday header names ─────────────────────────────────

    /// <summary>
    /// Returns 7 shortest day-name strings (1 char) starting from
    /// culture.DateTimeFormat.FirstDayOfWeek.
    /// </summary>
    public static string[] GetWeekDayHeaders(CultureInfo? culture = null)
    {
        culture ??= CultureInfo.CurrentUICulture;
        var dtf = culture.DateTimeFormat;
        var first = (int)dtf.FirstDayOfWeek;
        return Enumerable.Range(0, 7)
            .Select(i => dtf.GetShortestDayName((DayOfWeek)((first + i) % 7)))
            .ToArray();
    }

    /// <summary>
    /// Returns 7 abbreviated day-name strings (2–3 chars) starting from
    /// culture.DateTimeFormat.FirstDayOfWeek.
    /// </summary>
    public static string[] GetAbbreviatedWeekDayHeaders(CultureInfo? culture = null)
    {
        culture ??= CultureInfo.CurrentUICulture;
        var dtf = culture.DateTimeFormat;
        var first = (int)dtf.FirstDayOfWeek;
        return Enumerable.Range(0, 7)
            .Select(i => dtf.GetAbbreviatedDayName((DayOfWeek)((first + i) % 7)))
            .ToArray();
    }

    // ── Culture-aware: Calendar grid cells ──────────────────────────────────

    public static List<BlazorCalendarCell> GetCalendarCells(DateTime selectedDate, CultureInfo? culture = null)
    {
        culture ??= CultureInfo.CurrentUICulture;
        var cal = culture.Calendar;
        var dtf = culture.DateTimeFormat;

        int culturalYear  = cal.GetYear(selectedDate);
        int culturalMonth = cal.GetMonth(selectedDate);

        // First day of this cultural month as a Gregorian DateTime
        DateTime firstDay = cal.ToDateTime(culturalYear, culturalMonth, 1, 0, 0, 0, 0);
        int daysInMonth   = cal.GetDaysInMonth(culturalYear, culturalMonth);

        // Leading blank cells (days from prev cultural month)
        int firstDow      = (int)cal.GetDayOfWeek(firstDay);
        int culturalFirst = (int)dtf.FirstDayOfWeek;
        int leadingDays   = (firstDow - culturalFirst + 7) % 7;

        // Previous cultural month
        int prevCulturalMonth = culturalMonth == 1
            ? cal.GetMonthsInYear(culturalYear - 1)
            : culturalMonth - 1;
        int prevCulturalYear = culturalMonth == 1 ? culturalYear - 1 : culturalYear;
        int daysInPrevMonth  = cal.GetDaysInMonth(prevCulturalYear, prevCulturalMonth);

        var cells = new List<BlazorCalendarCell>();

        for (int i = 0; i < leadingDays; i++)
        {
            int d = daysInPrevMonth - leadingDays + i + 1;
            DateTime date = cal.ToDateTime(prevCulturalYear, prevCulturalMonth, d, 0, 0, 0, 0);
            cells.Add(new BlazorCalendarCell { Day = d, CurrentMonth = false, Date = date });
        }

        for (int i = 1; i <= daysInMonth; i++)
        {
            DateTime date = cal.ToDateTime(culturalYear, culturalMonth, i, 0, 0, 0, 0);
            cells.Add(new BlazorCalendarCell { Day = i, CurrentMonth = true, Date = date });
        }

        int totalDays  = leadingDays + daysInMonth;
        int trailing   = (7 - (totalDays % 7)) % 7;
        int nextCulturalMonth = culturalMonth == cal.GetMonthsInYear(culturalYear)
            ? 1
            : culturalMonth + 1;
        int nextCulturalYear = culturalMonth == cal.GetMonthsInYear(culturalYear)
            ? culturalYear + 1
            : culturalYear;

        for (int i = 1; i <= trailing; i++)
        {
            DateTime date = cal.ToDateTime(nextCulturalYear, nextCulturalMonth, i, 0, 0, 0, 0);
            cells.Add(new BlazorCalendarCell { Day = i, CurrentMonth = false, Date = date });
        }

        return cells;
    }

    // ── Culture-aware: Day-of-month display ─────────────────────────────────

    public static int GetCulturalDayOfMonth(DateTime date, CultureInfo? culture = null)
    {
        culture ??= CultureInfo.CurrentUICulture;
        return culture.Calendar.GetDayOfMonth(date);
    }

    // ── Culture-aware: Events for year ──────────────────────────────────────

    public static List<BlazorCalendarEvent> GetEventsForYear(List<BlazorCalendarEvent> events, DateTime date, CultureInfo? culture = null)
    {
        culture ??= CultureInfo.CurrentUICulture;
        var cal = culture.Calendar;
        int culturalYear  = cal.GetYear(date);
        DateTime yearStart = cal.ToDateTime(culturalYear, 1, 1, 0, 0, 0, 0);
        int monthsInYear   = cal.GetMonthsInYear(culturalYear);
        int lastDayOfYear  = cal.GetDaysInMonth(culturalYear, monthsInYear);
        DateTime yearEnd   = cal.ToDateTime(culturalYear, monthsInYear, lastDayOfYear, 23, 59, 59, 0);
        return events.Where(ev => ev.StartDate.Date <= yearEnd && ev.EndDate.Date >= yearStart).ToList();
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

    public static List<List<BlazorCalendarEvent>> GroupEvents(List<BlazorCalendarEvent> dayEvents)
    {
        var sorted = dayEvents.OrderBy(e => e.StartDate).ToList();
        var groups = new List<List<BlazorCalendarEvent>>();

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

    public static (double TopPx, double WidthPercent, double LeftPercent) GetEventBlockStyle(
        BlazorCalendarEvent ev, DateTime day, int groupIndex, int groupSize)
    {
        var dayStart = day.Date;
        var eventStart = ev.StartDate < dayStart ? dayStart : ev.StartDate;
        double startMinutes = (eventStart - dayStart).TotalMinutes;
        double topPx = startMinutes / 60.0 * HourHeightPx;
        double width = 100.0 / groupSize;
        double left = groupIndex * width;
        return (topPx, width, left);
    }

    private static (int Year, int Month, int Day) MonthGridDayKey(DateTime d)
    {
        d = d.Date;
        return (d.Year, d.Month, d.Day);
    }

    public static Dictionary<int, int> CalculateMonthEventPositions(
        List<BlazorCalendarEvent> multiDayEvents,
        List<BlazorCalendarEvent> singleDayEvents,
        DateTime selectedDate,
        CultureInfo? culture = null)
    {
        culture ??= CultureInfo.CurrentUICulture;
        var cal = culture.Calendar;
        int y = cal.GetYear(selectedDate);
        int m = cal.GetMonth(selectedDate);
        DateTime monthStart = cal.ToDateTime(y, m, 1, 0, 0, 0, 0);
        DateTime monthEnd   = cal.AddMonths(monthStart, 1).AddDays(-1);

        var eventPositions = new Dictionary<int, int>();
        var occupiedPositions = new Dictionary<(int Year, int Month, int Day), bool[]>();

        for (var d = monthStart; d <= monthEnd; d = d.AddDays(1))
            occupiedPositions[MonthGridDayKey(d)] = new bool[3];

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
                    var key = MonthGridDayKey(d);
                    return occupiedPositions.TryGetValue(key, out var slots) && !slots[i];
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
                    var key = MonthGridDayKey(d);
                    if (occupiedPositions.TryGetValue(key, out var slots))
                        slots[position] = true;
                }
                eventPositions[ev.Id] = position;
            }
        }

        return eventPositions;
    }

    public static List<(BlazorCalendarEvent Event, int Position, bool IsMultiDay)> GetMonthCellEvents(
        DateTime date, List<BlazorCalendarEvent> events, Dictionary<int, int> eventPositions)
    {
        var dayStart = date.Date;
        var eventsForDate = events.Where(ev =>
        {
            var s = ev.StartDate.Date;
            var e = ev.EndDate.Date;
            return (dayStart >= s && dayStart <= e) || s == dayStart || e == dayStart;
        }).ToList();

        var raw = eventsForDate
            .Select(ev => (
                Event: ev,
                Position: eventPositions.GetValueOrDefault(ev.Id, -1),
                IsMultiDay: ev.IsMultiDay
            ))
            .OrderByDescending(x => x.IsMultiDay)
            .ThenBy(x => x.Position < 0 ? 100 : x.Position)
            .ThenBy(x => x.Event.StartDate)
            .ToList();

        return AssignMonthCellDisplayRows(raw);
    }

    private static List<(BlazorCalendarEvent Event, int Position, bool IsMultiDay)> AssignMonthCellDisplayRows(
        List<(BlazorCalendarEvent Event, int Position, bool IsMultiDay)> raw)
    {
        var occupied = new bool[3];
        var result = new List<(BlazorCalendarEvent Event, int Position, bool IsMultiDay)>();

        foreach (var x in raw)
        {
            var p = x.Position;
            if (p is >= 0 and < 3 && !occupied[p])
            {
                occupied[p] = true;
                result.Add((x.Event, p, x.IsMultiDay));
                continue;
            }

            var free = -1;
            for (var i = 0; i < 3; i++)
            {
                if (!occupied[i])
                {
                    free = i;
                    break;
                }
            }

            if (free >= 0)
            {
                occupied[free] = true;
                result.Add((x.Event, free, x.IsMultiDay));
            }
            else
            {
                result.Add((x.Event, -1, x.IsMultiDay));
            }
        }

        return result
            .OrderByDescending(x => x.IsMultiDay)
            .ThenBy(x => x.Position < 0 ? 100 : x.Position)
            .ThenBy(x => x.Event.StartDate)
            .ToList();
    }

    public static List<BlazorCalendarEvent> GetEventsForDay(List<BlazorCalendarEvent> events, DateTime date, bool weekOnly = false)
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

    public static List<BlazorCalendarEvent> GetEventsForWeek(List<BlazorCalendarEvent> events, DateTime date, CultureInfo? culture = null)
    {
        var weekStart = StartOfWeek(date, culture);
        var weekEnd = weekStart.AddDays(6);
        return events.Where(ev =>
            ev.StartDate.Date <= weekEnd && ev.EndDate.Date >= weekStart).ToList();
    }

    public static List<BlazorCalendarEvent> GetEventsForMonth(List<BlazorCalendarEvent> events, DateTime date, CultureInfo? culture = null)
    {
        culture ??= CultureInfo.CurrentUICulture;
        var cal = culture.Calendar;
        int y = cal.GetYear(date);
        int m = cal.GetMonth(date);
        DateTime monthStart = cal.ToDateTime(y, m, 1, 0, 0, 0, 0);
        DateTime monthEnd   = cal.AddMonths(monthStart, 1).AddDays(-1);
        return events.Where(ev =>
            ev.StartDate.Date <= monthEnd && ev.EndDate.Date >= monthStart).ToList();
    }

    /// <summary>
    /// Events overlapping the date range implied by the current view and selected date
    /// (used for attendee filters and similar “in this view” logic).
    /// </summary>
    public static List<BlazorCalendarEvent> GetEventsForView(
        List<BlazorCalendarEvent> events,
        BlazorCalendarView view,
        DateTime selectedDate,
        CultureInfo? culture = null)
    {
        culture ??= CultureInfo.CurrentUICulture;
        return view switch
        {
            BlazorCalendarView.Day => GetEventsForDay(events, selectedDate),
            BlazorCalendarView.Week => GetEventsForWeek(events, selectedDate, culture),
            BlazorCalendarView.Month => GetEventsForMonth(events, selectedDate, culture),
            BlazorCalendarView.Year => GetEventsForYear(events, selectedDate, culture),
            BlazorCalendarView.Agenda => GetEventsForMonth(events, selectedDate, culture),
            _ => events.ToList()
        };
    }

    /// <summary>
    /// Smallest time t' &gt;= <paramref name="dt"/> on the same calendar day where
    /// (t' - t'.Date) is a whole multiple of <paramref name="intervalMinutes"/>.
    /// If <paramref name="dt"/> is already on such a boundary, returns <paramref name="dt"/> unchanged.
    /// </summary>
    public static DateTime CeilToMinuteInterval(DateTime dt, int intervalMinutes)
    {
        if (intervalMinutes <= 0)
            throw new ArgumentOutOfRangeException(nameof(intervalMinutes));

        var dayStart = dt.Date;
        var minutesSinceDay = (dt - dayStart).TotalMinutes;
        var slots = Math.Ceiling(minutesSinceDay / intervalMinutes);
        return dayStart.AddMinutes(slots * intervalMinutes);
    }

    /// <summary>
    /// Largest time t' &lt;= <paramref name="dt"/> on the same calendar day where
    /// (t' - t'.Date) is a whole multiple of <paramref name="intervalMinutes"/>.
    /// </summary>
    public static DateTime FloorToMinuteInterval(DateTime dt, int intervalMinutes)
    {
        if (intervalMinutes <= 0)
            throw new ArgumentOutOfRangeException(nameof(intervalMinutes));

        var dayStart = dt.Date;
        var minutesSinceDay = (dt - dayStart).TotalMinutes;
        var slots = Math.Floor(minutesSinceDay / intervalMinutes);
        return dayStart.AddMinutes(slots * intervalMinutes);
    }

    /// <summary>Stable key for filtering events by attendee (Id preferred, else full name).</summary>
    public static string AttendeeFilterKey(BlazorCalendarAttendee a)
    {
        if (!string.IsNullOrWhiteSpace(a.Id))
            return "id:" + a.Id.Trim();
        if (!string.IsNullOrWhiteSpace(a.FullName))
            return "name:" + a.FullName.Trim().ToLowerInvariant();
        return "";
    }

    public static string GetColorCss(BlazorCalendarEventColor color) => color switch
    {
        BlazorCalendarEventColor.Blue => "cal-color-blue",
        BlazorCalendarEventColor.Green => "cal-color-green",
        BlazorCalendarEventColor.Red => "cal-color-red",
        BlazorCalendarEventColor.Yellow => "cal-color-yellow",
        BlazorCalendarEventColor.Purple => "cal-color-purple",
        BlazorCalendarEventColor.Orange => "cal-color-orange",
        _ => "cal-color-blue"
    };

    public static string GetBgColorCss(BlazorCalendarEventColor color) => color switch
    {
        BlazorCalendarEventColor.Blue => "cal-bg-blue",
        BlazorCalendarEventColor.Green => "cal-bg-green",
        BlazorCalendarEventColor.Red => "cal-bg-red",
        BlazorCalendarEventColor.Yellow => "cal-bg-yellow",
        BlazorCalendarEventColor.Purple => "cal-bg-purple",
        BlazorCalendarEventColor.Orange => "cal-bg-orange",
        _ => "cal-bg-blue"
    };

    public static double GetCurrentTimeLineTopPx()
    {
        double minutes = DateTime.Now.TimeOfDay.TotalMinutes;
        return minutes / 60.0 * HourHeightPx;
    }

    public static string Capitalize(string str)
    {
        if (string.IsNullOrEmpty(str)) return "";
        return char.ToUpperInvariant(str[0]) + str[1..];
    }
}
