namespace BlazorCalendar.Demo;

public static class DemoSampleData
{
    private static BlazorCalendarAttendee A(string first, string last, string id) =>
        new() { FirstName = first, LastName = last, Id = id };

    private static readonly BlazorCalendarAttendee Alice   = A("Alice",  "Johnson",  "1");
    private static readonly BlazorCalendarAttendee Bob     = A("Bob",    "Smith",    "2");
    private static readonly BlazorCalendarAttendee Carol   = A("Carol",  "Davis",    "3");
    private static readonly BlazorCalendarAttendee David   = A("David",  "Lee",      "4");
    private static readonly BlazorCalendarAttendee Eva     = A("Eva",    "Martinez", "5");
    private static readonly BlazorCalendarAttendee Frank   = A("Frank",  "Chen",     "6");

    public static List<BlazorCalendarEvent> CreateEvents()
    {
        var today = DateTime.Today;
        var events = new List<BlazorCalendarEvent>();
        var id = 1;

        events.Add(new() { Id = id++, Title = "Team Standup",        Description = "Daily sync with engineering.",               StartDate = today.AddHours(8),                     EndDate = today.AddHours(8).AddMinutes(45),  Color = BlazorCalendarEventColor.Blue,   Attendees = [Alice, Bob, Carol] });
        events.Add(new() { Id = id++, Title = "Inbox zero",           Description = "Clear urgent email and Slack.",             StartDate = today.AddHours(8).AddMinutes(30),      EndDate = today.AddHours(9),                 Color = BlazorCalendarEventColor.Yellow, Attendees = [Frank] });
        events.Add(new() { Id = id++, Title = "Team Standup (EU)",    Description = "Overlap standup for EU squad.",             StartDate = today.AddHours(9),                     EndDate = today.AddHours(9).AddMinutes(30),  Color = BlazorCalendarEventColor.Purple, Attendees = [Bob, Eva] });
        events.Add(new() { Id = id++, Title = "Design Review",        Description = "Dashboard mockups v2.",                     StartDate = today.AddHours(10),                    EndDate = today.AddHours(11),                Color = BlazorCalendarEventColor.Purple, Attendees = [Bob, Alice] });
        events.Add(new() { Id = id++, Title = "1:1 with Manager",     Description = "Career and sprint check-in.",              StartDate = today.AddHours(10).AddMinutes(30),     EndDate = today.AddHours(11).AddMinutes(15), Color = BlazorCalendarEventColor.Yellow, Attendees = [Alice] });
        events.Add(new() { Id = id++, Title = "Coffee chat",          Description = "Informal catch-up with design.",           StartDate = today.AddHours(11),                    EndDate = today.AddHours(11).AddMinutes(30), Color = BlazorCalendarEventColor.Green,  Attendees = [Carol, Frank] });
        events.Add(new() { Id = id++, Title = "Lunch with Client",    Description = "Q3 roadmap discussion.",                   StartDate = today.AddHours(12),                    EndDate = today.AddHours(13).AddMinutes(30), Color = BlazorCalendarEventColor.Green,  Attendees = [Carol, David] });
        events.Add(new() { Id = id++, Title = "Focus block",          Description = "Deep work — notifications off.",           StartDate = today.AddHours(13),                    EndDate = today.AddHours(14),                Color = BlazorCalendarEventColor.Blue,   Attendees = [Frank] });
        events.Add(new() { Id = id++, Title = "Sprint Planning",      Description = "Next sprint goals and capacity.",          StartDate = today.AddHours(14),                    EndDate = today.AddHours(15).AddMinutes(30), Color = BlazorCalendarEventColor.Orange, Attendees = [David, Alice, Bob] });
        events.Add(new() { Id = id++, Title = "API pairing",          Description = "Implement rate limiting together.",        StartDate = today.AddHours(15),                    EndDate = today.AddHours(16).AddMinutes(30), Color = BlazorCalendarEventColor.Red,    Attendees = [Bob, Frank] });
        events.Add(new() { Id = id++, Title = "Code Review Session",  Description = "Auth module PRs.",                        StartDate = today.AddHours(16),                    EndDate = today.AddHours(17),                Color = BlazorCalendarEventColor.Red,    Attendees = [Bob] });
        events.Add(new() { Id = id++, Title = "Run club",             Description = "5k easy loop near office.",               StartDate = today.AddHours(17),                    EndDate = today.AddHours(17).AddMinutes(45), Color = BlazorCalendarEventColor.Green,  Attendees = [Eva, Carol] });
        events.Add(new() { Id = id++, Title = "Yoga Class",           Description = "Company wellness session.",               StartDate = today.AddHours(17).AddMinutes(30),     EndDate = today.AddHours(18).AddMinutes(30), Color = BlazorCalendarEventColor.Green,  Attendees = [Eva] });
        events.Add(new() { Id = id++, Title = "Docs & handoff",       Description = "Write release notes draft.",              StartDate = today.AddHours(18),                    EndDate = today.AddHours(18).AddMinutes(45), Color = BlazorCalendarEventColor.Purple, Attendees = [Alice] });

        events.Add(new() { Id = id++, Title = "Tech Conference 2026", Description = "Keynotes, workshops, and hallway track.", StartDate = today.AddDays(1).AddHours(9),          EndDate = today.AddDays(3).AddHours(17),     Color = BlazorCalendarEventColor.Blue,   Attendees = [Alice, Bob, Carol, David] });
        events.Add(new() { Id = id++, Title = "Company Retreat",      Description = "Lake house — strategy and team building.",StartDate = today.AddDays(5),                      EndDate = today.AddDays(7).AddHours(16),     Color = BlazorCalendarEventColor.Purple, Attendees = [Carol, Eva, Frank] });
        events.Add(new() { Id = id++, Title = "Hackathon",            Description = "48-hour build sprint.",                   StartDate = today.AddDays(12).AddHours(9),         EndDate = today.AddDays(13).AddHours(17),    Color = BlazorCalendarEventColor.Orange, Attendees = [David, Bob] });
        events.Add(new() { Id = id++, Title = "Sales kickoff",        Description = "Multi-day SKO for AMER.",                 StartDate = today.AddDays(18).AddHours(8),         EndDate = today.AddDays(20).AddHours(17),    Color = BlazorCalendarEventColor.Orange, Attendees = [Eva, Alice] });

        events.Add(new() { Id = id++, Title = "Client Onboarding",    Description = "Platform walkthrough for new tenant.",    StartDate = today.AddDays(1).AddHours(10),         EndDate = today.AddDays(1).AddHours(11).AddMinutes(30), Color = BlazorCalendarEventColor.Yellow, Attendees = [Eva, Carol] });
        events.Add(new() { Id = id++, Title = "Architecture Review",  Description = "Microservices migration plan.",           StartDate = today.AddDays(1).AddHours(14),         EndDate = today.AddDays(1).AddHours(16),     Color = BlazorCalendarEventColor.Red,    Attendees = [Bob, David, Frank] });
        events.Add(new() { Id = id++, Title = "Evening webinar",      Description = "Blazor performance tips (public).",      StartDate = today.AddDays(1).AddHours(19),         EndDate = today.AddDays(1).AddHours(20),     Color = BlazorCalendarEventColor.Blue,   Attendees = [Frank] });

        events.Add(new() { Id = id++, Title = "Marketing Sync",       Description = "Launch campaign coordination.",          StartDate = today.AddDays(2).AddHours(10),         EndDate = today.AddDays(2).AddHours(11),     Color = BlazorCalendarEventColor.Green,  Attendees = [Carol, Alice] });
        events.Add(new() { Id = id++, Title = "Blazor Workshop",      Description = "Components, state, and interop.",        StartDate = today.AddDays(2).AddHours(13),         EndDate = today.AddDays(2).AddHours(16),     Color = BlazorCalendarEventColor.Purple, Attendees = [Alice, Bob, Frank] });
        events.Add(new() { Id = id++, Title = "Budget checkpoint",    Description = "Finance Q&A for engineering spend.",     StartDate = today.AddDays(2).AddHours(15),         EndDate = today.AddDays(2).AddHours(15).AddMinutes(45), Color = BlazorCalendarEventColor.Yellow, Attendees = [David] });

        events.Add(new() { Id = id++, Title = "Database Migration",   Description = "Prod cutover — maintenance window.",     StartDate = today.AddDays(3).AddHours(22),         EndDate = today.AddDays(4).AddHours(2),      Color = BlazorCalendarEventColor.Red,    Attendees = [Bob, Frank] });
        events.Add(new() { Id = id++, Title = "UX Testing",           Description = "Checkout usability sessions.",           StartDate = today.AddDays(4).AddHours(10),         EndDate = today.AddDays(4).AddHours(12),     Color = BlazorCalendarEventColor.Yellow, Attendees = [Eva, Carol] });
        events.Add(new() { Id = id++, Title = "Friday demos",         Description = "Sprint review + live demos.",            StartDate = today.AddDays(5).AddHours(14),         EndDate = today.AddDays(5).AddHours(16),     Color = BlazorCalendarEventColor.Blue,   Attendees = [Alice, David] });
        events.Add(new() { Id = id++, Title = "Support rotation handoff", Description = "PagerDuty and runbooks.",           StartDate = today.AddDays(6).AddHours(9),          EndDate = today.AddDays(6).AddHours(9).AddMinutes(30), Color = BlazorCalendarEventColor.Orange, Attendees = [Frank] });

        events.Add(new() { Id = id++, Title = "Quarterly Review",     Description = "Company-wide QBR.",                     StartDate = today.AddDays(7).AddHours(10),         EndDate = today.AddDays(7).AddHours(12),     Color = BlazorCalendarEventColor.Red,    Attendees = [Bob, Alice, Carol, David, Eva] });
        events.Add(new() { Id = id++, Title = "Investor Pitch",       Description = "Series B deck walkthrough.",            StartDate = today.AddDays(8).AddHours(14),         EndDate = today.AddDays(8).AddHours(15).AddMinutes(30), Color = BlazorCalendarEventColor.Orange, Attendees = [Alice, Frank] });
        events.Add(new() { Id = id++, Title = "Security Audit",       Description = "External pen test readout.",            StartDate = today.AddDays(9).AddHours(9),          EndDate = today.AddDays(9).AddHours(17),     Color = BlazorCalendarEventColor.Red,    Attendees = [Bob, David] });
        events.Add(new() { Id = id++, Title = "Team Outing",          Description = "Escape room + dinner.",                 StartDate = today.AddDays(10).AddHours(15),        EndDate = today.AddDays(10).AddHours(20),    Color = BlazorCalendarEventColor.Green,  Attendees = [David, Eva, Frank, Carol] });
        events.Add(new() { Id = id++, Title = "Open office hours",    Description = "Staff engineers — drop-in questions.",  StartDate = today.AddDays(11).AddHours(13),        EndDate = today.AddDays(11).AddHours(14),    Color = BlazorCalendarEventColor.Purple, Attendees = [Carol, Bob] });

        events.Add(new() { Id = id++, Title = "Product Demo",         Description = "Stakeholder feature walkthrough.",      StartDate = today.AddDays(-2).AddHours(14),        EndDate = today.AddDays(-2).AddHours(15),    Color = BlazorCalendarEventColor.Orange, Attendees = [Alice, Eva] });
        events.Add(new() { Id = id++, Title = "Board Meeting",        Description = "Monthly board session.",                StartDate = today.AddDays(-5).AddHours(10),        EndDate = today.AddDays(-5).AddHours(12),    Color = BlazorCalendarEventColor.Purple, Attendees = [Carol, Alice, David] });
        events.Add(new() { Id = id++, Title = "Training Day",         Description = "New hire orientation.",                 StartDate = today.AddDays(-7).AddHours(9),         EndDate = today.AddDays(-7).AddHours(17),    Color = BlazorCalendarEventColor.Blue,   Attendees = [Eva, Frank] });
        events.Add(new() { Id = id++, Title = "Postmortem",           Description = "Incident review — action items.",       StartDate = today.AddDays(-10).AddHours(15),       EndDate = today.AddDays(-10).AddHours(16).AddMinutes(30), Color = BlazorCalendarEventColor.Red, Attendees = [Bob, Alice] });

        events.Add(new() { Id = id++, Title = "Deadline: API v2",     Description = "API v2 release — feature freeze.",     StartDate = today.AddDays(14).AddHours(9),         EndDate = today.AddDays(14).AddHours(17),    Color = BlazorCalendarEventColor.Red,    Attendees = [Bob, Frank, David] });
        events.Add(new() { Id = id++, Title = "Performance Reviews",  Description = "Mid-year review cycle.",                StartDate = today.AddDays(20).AddHours(9),         EndDate = today.AddDays(22).AddHours(17),    Color = BlazorCalendarEventColor.Yellow, Attendees = [David, Carol] });
        events.Add(new() { Id = id++, Title = "Holiday Party",        Description = "Awards and celebration.",               StartDate = today.AddDays(25).AddHours(17),        EndDate = today.AddDays(25).AddHours(21),    Color = BlazorCalendarEventColor.Purple, Attendees = [Eva, Alice, Bob, Carol, David, Frank] });
        events.Add(new() { Id = id++, Title = "Product Launch",       Description = "v3.0 go-live.",                        StartDate = today.AddDays(28).AddHours(10),        EndDate = today.AddDays(28).AddHours(12),    Color = BlazorCalendarEventColor.Green,  Attendees = [Alice, Bob, David] });

        var allAttendees = new[] { Alice, Bob, Carol, David, Eva, Frank };
        for (var d = -14; d <= 28; d += 3)
        {
            if (d is 0 or 1 or 2) continue;
            var day = today.AddDays(d);
            var hourBase = 9 + (Math.Abs(d) % 5);
            events.Add(new()
            {
                Id = id++,
                Title = $"Workshop block ({day:MMM d})",
                Description = "Reserved focus time for docs and spikes.",
                StartDate = day.AddHours(hourBase),
                EndDate = day.AddHours(hourBase + 1),
                Color = (BlazorCalendarEventColor)(Math.Abs(d) % 6),
                Attendees = [allAttendees[Math.Abs(d) % allAttendees.Length]]
            });
        }

        return events;
    }
}
