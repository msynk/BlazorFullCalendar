# BlazorCalendar

A feature-rich, interactive calendar component for Blazor applications. Built with pure Blazor and .NET - no JavaScript frameworks required.

## Features

- **5 View Modes**: Day, Week, Month, Year, and Agenda views with smooth transitions
- **Event Management**: Create, edit, and delete events with a polished dialog and form validation
- **Drag & Drop**: Move events between time slots and dates with native HTML5 drag-and-drop
- **Multi-User Support**: Filter events by user or color with avatar initials and color badges
- **Customizable**: Dark mode, 12/24-hour format, dot vs colored badges, configurable start hour, and agenda grouping options
- **Live Timeline**: Real-time current-time indicator in day and week views with "Happening Now" sidebar

## Installation

1. Add the BlazorCalendar project reference to your Blazor application
2. Register the Razor Class Library assembly in your `Program.cs`:

```csharp
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddAdditionalAssemblies(BlazorCalendar.BlazorCalendarAssembly.Value);
```

3. Add the namespace import to your `_Imports.razor`:

```razor
@using BlazorCalendar.Models
@using BlazorCalendar.Services
@using BlazorCalendar.Components.Calendar
```

## Usage

### Basic Example

```razor
@page "/calendar"

<BlazorCalendar Events="myEvents" Users="myUsers" @rendermode="InteractiveServer" />

@code {
    private List<CalendarEvent> myEvents = new()
    {
        new() {
            Id = 1,
            Title = "Team Meeting",
            Description = "Weekly sync",
            StartDate = DateTime.Today.AddHours(10),
            EndDate = DateTime.Today.AddHours(11),
            Color = EventColor.Blue,
            User = new() { Id = "1", Name = "Alice" }
        }
    };

    private List<CalendarUser> myUsers = new()
    {
        new() { Id = "1", Name = "Alice" }
    };
}
```

### Models

#### CalendarEvent

```csharp
public class CalendarEvent
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public EventColor Color { get; set; }
    public CalendarUser User { get; set; }
}
```

#### CalendarUser

```csharp
public class CalendarUser
{
    public string Id { get; set; }
    public string Name { get; set; }
}
```

#### EventColor

Available colors: `Blue`, `Green`, `Red`, `Yellow`, `Purple`, `Orange`

## Component Parameters

| Parameter | Type | Description |
|-----------|------|-------------|
| `Events` | `List<CalendarEvent>?` | List of calendar events to display |
| `Users` | `List<CalendarUser>?` | List of users for event assignment and filtering |

## Views

- **Month View**: Grid layout with multi-day event support
- **Week View**: 7-day view with hourly time slots
- **Day View**: Single-day detailed view with timeline
- **Year View**: 12-month overview with event indicators
- **Agenda View**: List view grouped by date or user

## Customization

The calendar includes built-in settings accessible via the settings button:
- Toggle dark mode
- Switch between 12/24-hour time format
- Choose badge style (colored or dots)
- Set start hour for day/week views
- Configure agenda view grouping

## Browser Support

- Modern browsers with CSS Grid and Flexbox support
- HTML5 Drag and Drop API support recommended

## Credits

Original concept inspired by [yassir-jeraidi/full-calendar](https://github.com/yassir-jeraidi/full-calendar)

## License

MIT, use it and be happy :)
