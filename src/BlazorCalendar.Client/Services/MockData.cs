using BlazorCalendar.Client.Models;

namespace BlazorCalendar.Client.Services;

public static class MockData
{
    private static readonly CalendarUser[] Users =
    [
        new() { Id = "1", Name = "Alice Johnson", PicturePath = null },
        new() { Id = "2", Name = "Bob Smith", PicturePath = null },
        new() { Id = "3", Name = "Carol Davis", PicturePath = null },
        new() { Id = "4", Name = "David Lee", PicturePath = null },
        new() { Id = "5", Name = "Eva Martinez", PicturePath = null },
    ];

    public static List<CalendarUser> GetUsers() => [.. Users];

    public static List<CalendarEvent> GetEvents()
    {
        var today = DateTime.Today;
        var events = new List<CalendarEvent>();
        int id = 1;

        // --- Today's events (variety of durations and overlaps) ---
        events.Add(new CalendarEvent
        {
            Id = id++, Title = "Team Standup", Description = "Daily sync with the engineering team. Review blockers and progress.",
            StartDate = today.AddHours(9), EndDate = today.AddHours(9).AddMinutes(30),
            Color = EventColor.Blue, User = Users[0]
        });
        events.Add(new CalendarEvent
        {
            Id = id++, Title = "Design Review", Description = "Review new dashboard mockups with the design team.",
            StartDate = today.AddHours(10), EndDate = today.AddHours(11),
            Color = EventColor.Purple, User = Users[1]
        });
        events.Add(new CalendarEvent
        {
            Id = id++, Title = "1:1 with Manager", Description = "Weekly sync to discuss career growth and current sprint.",
            StartDate = today.AddHours(10).AddMinutes(30), EndDate = today.AddHours(11).AddMinutes(15),
            Color = EventColor.Yellow, User = Users[0]
        });
        events.Add(new CalendarEvent
        {
            Id = id++, Title = "Lunch with Client", Description = "Discuss Q3 roadmap and partnership opportunities.",
            StartDate = today.AddHours(12), EndDate = today.AddHours(13).AddMinutes(30),
            Color = EventColor.Green, User = Users[2]
        });
        events.Add(new CalendarEvent
        {
            Id = id++, Title = "Sprint Planning", Description = "Plan next sprint tasks, assign story points, and prioritize backlog.",
            StartDate = today.AddHours(14), EndDate = today.AddHours(15).AddMinutes(30),
            Color = EventColor.Orange, User = Users[3]
        });
        events.Add(new CalendarEvent
        {
            Id = id++, Title = "Code Review Session", Description = "Review pull requests for the authentication module.",
            StartDate = today.AddHours(16), EndDate = today.AddHours(17),
            Color = EventColor.Red, User = Users[1]
        });
        events.Add(new CalendarEvent
        {
            Id = id++, Title = "Yoga Class", Description = "Wellness break — company-sponsored yoga session.",
            StartDate = today.AddHours(17).AddMinutes(30), EndDate = today.AddHours(18).AddMinutes(30),
            Color = EventColor.Green, User = Users[4]
        });

        // --- Multi-day events ---
        events.Add(new CalendarEvent
        {
            Id = id++, Title = "Tech Conference 2026", Description = "Annual technology conference with keynotes, workshops, and networking.",
            StartDate = today.AddDays(1).AddHours(9), EndDate = today.AddDays(3).AddHours(17),
            Color = EventColor.Blue, User = Users[0]
        });
        events.Add(new CalendarEvent
        {
            Id = id++, Title = "Company Retreat", Description = "Team retreat at the lake house. Strategy sessions and team bonding.",
            StartDate = today.AddDays(5), EndDate = today.AddDays(7).AddHours(16),
            Color = EventColor.Purple, User = Users[2]
        });
        events.Add(new CalendarEvent
        {
            Id = id++, Title = "Hackathon", Description = "48-hour company hackathon. Build something amazing!",
            StartDate = today.AddDays(12).AddHours(9), EndDate = today.AddDays(13).AddHours(17),
            Color = EventColor.Orange, User = Users[3]
        });

        // --- Tomorrow ---
        events.Add(new CalendarEvent
        {
            Id = id++, Title = "Client Onboarding", Description = "Walk new client through platform features and setup.",
            StartDate = today.AddDays(1).AddHours(10), EndDate = today.AddDays(1).AddHours(11).AddMinutes(30),
            Color = EventColor.Yellow, User = Users[4]
        });
        events.Add(new CalendarEvent
        {
            Id = id++, Title = "Architecture Review", Description = "Discuss microservices migration plan with senior engineers.",
            StartDate = today.AddDays(1).AddHours(14), EndDate = today.AddDays(1).AddHours(16),
            Color = EventColor.Red, User = Users[1]
        });

        // --- Day after tomorrow ---
        events.Add(new CalendarEvent
        {
            Id = id++, Title = "Blazor Workshop", Description = "Hands-on Blazor workshop covering components, state, and interop.",
            StartDate = today.AddDays(2).AddHours(13), EndDate = today.AddDays(2).AddHours(16),
            Color = EventColor.Purple, User = Users[0]
        });
        events.Add(new CalendarEvent
        {
            Id = id++, Title = "Marketing Sync", Description = "Coordinate launch campaign with the marketing team.",
            StartDate = today.AddDays(2).AddHours(10), EndDate = today.AddDays(2).AddHours(11),
            Color = EventColor.Green, User = Users[2]
        });

        // --- This week (various days) ---
        events.Add(new CalendarEvent
        {
            Id = id++, Title = "Database Migration", Description = "Migrate production database to new cluster. Maintenance window.",
            StartDate = today.AddDays(3).AddHours(22), EndDate = today.AddDays(4).AddHours(2),
            Color = EventColor.Red, User = Users[1]
        });
        events.Add(new CalendarEvent
        {
            Id = id++, Title = "UX Testing", Description = "Usability testing with 5 participants for the new checkout flow.",
            StartDate = today.AddDays(4).AddHours(10), EndDate = today.AddDays(4).AddHours(12),
            Color = EventColor.Yellow, User = Users[4]
        });

        // --- Next week ---
        events.Add(new CalendarEvent
        {
            Id = id++, Title = "Quarterly Review", Description = "Company-wide quarterly business review. All hands meeting.",
            StartDate = today.AddDays(7).AddHours(10), EndDate = today.AddDays(7).AddHours(12),
            Color = EventColor.Red, User = Users[1]
        });
        events.Add(new CalendarEvent
        {
            Id = id++, Title = "Investor Pitch", Description = "Present Series B pitch deck to potential investors.",
            StartDate = today.AddDays(8).AddHours(14), EndDate = today.AddDays(8).AddHours(15).AddMinutes(30),
            Color = EventColor.Orange, User = Users[0]
        });
        events.Add(new CalendarEvent
        {
            Id = id++, Title = "Team Outing", Description = "Team building activity — escape room and dinner.",
            StartDate = today.AddDays(10).AddHours(15), EndDate = today.AddDays(10).AddHours(20),
            Color = EventColor.Green, User = Users[3]
        });
        events.Add(new CalendarEvent
        {
            Id = id++, Title = "Security Audit", Description = "External security audit of the payment processing module.",
            StartDate = today.AddDays(9).AddHours(9), EndDate = today.AddDays(9).AddHours(17),
            Color = EventColor.Red, User = Users[1]
        });

        // --- Past events (for month/year views) ---
        events.Add(new CalendarEvent
        {
            Id = id++, Title = "Product Demo", Description = "Demo new features to stakeholders and collect feedback.",
            StartDate = today.AddDays(-2).AddHours(14), EndDate = today.AddDays(-2).AddHours(15),
            Color = EventColor.Orange, User = Users[0]
        });
        events.Add(new CalendarEvent
        {
            Id = id++, Title = "Board Meeting", Description = "Monthly board of directors meeting.",
            StartDate = today.AddDays(-5).AddHours(10), EndDate = today.AddDays(-5).AddHours(12),
            Color = EventColor.Purple, User = Users[2]
        });
        events.Add(new CalendarEvent
        {
            Id = id++, Title = "Training Day", Description = "New hire orientation and onboarding training.",
            StartDate = today.AddDays(-7).AddHours(9), EndDate = today.AddDays(-7).AddHours(17),
            Color = EventColor.Blue, User = Users[4]
        });

        // --- Further out (for year view) ---
        events.Add(new CalendarEvent
        {
            Id = id++, Title = "Deadline: API v2", Description = "API v2 release deadline. Feature freeze.",
            StartDate = today.AddDays(14).AddHours(9), EndDate = today.AddDays(14).AddHours(17),
            Color = EventColor.Red, User = Users[1]
        });
        events.Add(new CalendarEvent
        {
            Id = id++, Title = "Performance Reviews", Description = "Mid-year performance review cycle.",
            StartDate = today.AddDays(20).AddHours(9), EndDate = today.AddDays(22).AddHours(17),
            Color = EventColor.Yellow, User = Users[3]
        });
        events.Add(new CalendarEvent
        {
            Id = id++, Title = "Product Launch", Description = "Official launch of v3.0 to production.",
            StartDate = today.AddDays(28).AddHours(10), EndDate = today.AddDays(28).AddHours(12),
            Color = EventColor.Green, User = Users[0]
        });
        events.Add(new CalendarEvent
        {
            Id = id++, Title = "Holiday Party", Description = "End of month celebration and awards ceremony.",
            StartDate = today.AddDays(25).AddHours(17), EndDate = today.AddDays(25).AddHours(21),
            Color = EventColor.Purple, User = Users[4]
        });

        return events;
    }
}
