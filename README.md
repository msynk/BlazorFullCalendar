# BlazorFullCalendar

A feature-rich, interactive calendar component for Blazor applications. Built with pure Blazor and .NET - no JavaScript frameworks required.

## Features

- **5 View Modes**: Day, Week, Month, Year, and Agenda views with smooth transitions
- **Event Management**: Create, edit, and delete events with a polished dialog and form validation
- **Culture-Aware Date-Time Picker**: Built-in dropdown date-time picker in add/edit dialogs (no browser-native `datetime-local`) with culture calendar rendering support (including `fa-IR`)
- **Drag & Drop**: Move events between time slots and dates with native HTML5 drag-and-drop
- **Multi-User Support**: Filter events by user or color with avatar initials and color badges
- **Text Customization**: Override UI labels, button text, placeholders, aria labels, and validation messages with `BlazorFullCalendarTexts`
- **Customizable**: Dark mode, 12/24-hour format, dot vs colored badges, configurable start hour, and agenda grouping options
- **Live Timeline**: Real-time current-time indicator in day and week views with "Happening Now" sidebar

## Installation

1. Install the [NuGet package](https://www.nuget.org/packages/BlazorFullCalendar) (or add a project reference to this repository):

```bash
dotnet add package BlazorFullCalendar
```

2. Register the Razor Class Library assembly in your `Program.cs`:

```csharp
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddAdditionalAssemblies(BlazorFullCalendar.BlazorFullCalendarAssembly.Value);
```

3. Add the namespace imports to your `_Imports.razor`:

```razor
@using BlazorFullCalendar
```

## Usage

### Basic Example

```razor
@page "/calendar"

<BlazorFullCalendar Events="myEvents"
                OnChange="HandleCalendarChange"
                @rendermode="InteractiveServer" />

@code {
    private List<BlazorFullCalendarEvent> myEvents = new()
    {
        new() {
            Id = 1,
            Title = "Team Meeting",
            Description = "Weekly sync",
            StartDate = DateTime.Today.AddHours(10),
            EndDate = DateTime.Today.AddHours(11),
            Color = BlazorFullCalendarEventColor.Blue
        }
    };

    private Task HandleCalendarChange(BlazorFullCalendarChangeEventArgs args)
    {
        // Persist args.Event or synchronize with your backend/store.
        return Task.CompletedTask;
    }
}
```

### Localization Notes

- The event add/edit dialog uses a custom dropdown date-time picker instead of native browser date inputs.
- Date cells, weekday headers, month names, and year/day values are rendered from the active `CultureInfo` calendar.
- This improves consistency for non-Gregorian cultures such as Persian (`fa-IR`) and other localized calendars.
- Dialog labels and validation text can be localized by supplying a customized `BlazorFullCalendarTexts` instance.

### Text Customization Example

```razor
<BlazorFullCalendar Events="myEvents"
                CultureName="fa-IR"
                Texts="calendarTexts"
                @rendermode="InteractiveServer" />

@code {
    private readonly BlazorFullCalendarTexts calendarTexts = new()
    {
        AddEventButton = "افزودن رویداد",
        AddEventDialogTitle = "افزودن رویداد جدید",
        StartDateTimeLabel = "تاریخ و زمان شروع",
        EndDateTimeLabel = "تاریخ و زمان پایان",
        CreateEventButton = "ایجاد رویداد"
    };
}
```

### Models

#### BlazorFullCalendarEvent

```csharp
public class BlazorFullCalendarEvent
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public BlazorFullCalendarEventColor Color { get; set; }
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

#### BlazorFullCalendarEventColor

Available colors: `Blue`, `Green`, `Red`, `Yellow`, `Purple`, `Orange`

## Component Parameters

| Parameter | Type | Description |
|-----------|------|-------------|
| `Events` | `List<BlazorFullCalendarEvent>?` | List of calendar events to display |
| `Users` | `List<CalendarUser>?` | List of users for event assignment and filtering |
| `Texts` | `BlazorFullCalendarTexts` | Custom UI strings for labels, placeholders, action buttons, aria labels, and validation messages |
| `Culture` | `CultureInfo?` | Sets calendar/date rendering and formatting |
| `CultureName` | `string?` | Culture name shortcut (for example `fa-IR`, `ar-SA`, `fr-FR`) |
| `OnChange` | `EventCallback<BlazorFullCalendarChangeEventArgs>` | Raised when a user adds, edits, or deletes an event (`Kind`: `Add`, `Edit`, `Delete`) |

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

