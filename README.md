# BlazorFullCalendar

A feature-rich, interactive calendar component for Blazor applications. Built with pure Blazor and .NET — no JavaScript frameworks required.

## Features

- **5 View Modes**: Day, Week, Month, Year, and Agenda views with smooth transitions
- **Event Management**: Create, edit, and delete events with a polished dialog and form validation
- **Custom Add/Edit UI (`OnAddOrEditClick`)**: Suppress the built-in dialog entirely and receive a draft or cloned event so you can show your own creation/editing experience
- **Custom Event Click (`OnEventClick`)**: Suppress the built-in event details dialog and handle event clicks yourself
- **Culture-Aware Date-Time Picker**: Built-in dropdown date-time picker in add/edit dialogs (no browser-native `datetime-local`) with culture calendar rendering support (including `fa-IR`)
- **Drag & Drop**: Move events between time slots and dates with native HTML5 drag-and-drop
- **Resize**: Drag the top or bottom handle of any day/week event block to adjust its start or end time
- **Multi-User Support**: Filter events by attendee or color with avatar initials and color badges
- **Text Customization**: Override UI labels, button text, placeholders, aria labels, and validation messages with `BlazorFullCalendarTexts`
- **Customizable**: Dark mode, 12/24-hour format, dot vs colored badges, configurable start hour, and agenda grouping options
- **Live Timeline**: Real-time current-time indicator in day and week views with "Happening Now" sidebar
- **Themes**: Default and Fluent (WinUI-style) built-in themes; dark mode supported for both
- **Self-Loading Assets**: The component can inject its own CSS and JS automatically — no manual `<link>` or `<script>` tags required

## Installation

### 1. Install the NuGet package

```bash
dotnet add package BlazorFullCalendar
```

Or add a project reference if you are working from this repository.

### 2. Register the assembly

**Blazor Server / Interactive Server**

```csharp
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddAdditionalAssemblies(BlazorFullCalendar.BlazorFullCalendarAssembly.Value);
```

**Blazor WebAssembly**

```csharp
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
// The assembly is discovered automatically for WASM.
```

### 3. Add the namespace import

In your `_Imports.razor`:

```razor
@using BlazorFullCalendar
```

### 4. Asset loading

By default the component automatically injects its stylesheet and JavaScript into the page the first time it renders (`LoadAssets="true"`). **No extra tags are needed in your host page.**

If you prefer to control asset loading yourself — for example to set a specific load order, use a bundler, or serve the files from a CDN — set `LoadAssets="false"` and add the tags manually to your host page (`index.html` or `App.razor`):

```html
<link rel="stylesheet" href="_content/BlazorFullCalendar/css/blazor-fullcalendar.css" />
<script src="_content/BlazorFullCalendar/js/blazor-fullcalendar.js"></script>
```

> The Fluent theme overrides are bundled inside `blazor-fullcalendar.css` and activated automatically when `Theme="BlazorFullCalendarTheme.Fluent"` is set on the component — no second stylesheet is required.

---

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
            Id = "1",
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

### Fluent Theme Example

```razor
<BlazorFullCalendar Events="myEvents"
                    Theme="BlazorFullCalendarTheme.Fluent"
                    OnChange="HandleCalendarChange"
                    @rendermode="InteractiveServer" />
```

### Manual Asset Loading Example

```razor
<!-- In your host page when LoadAssets="false" -->
<link rel="stylesheet" href="_content/BlazorFullCalendar/css/blazor-fullcalendar.css" />
<script src="_content/BlazorFullCalendar/js/blazor-fullcalendar.js"></script>
```

```razor
<BlazorFullCalendar Events="myEvents"
                    LoadAssets="false"
                    OnChange="HandleCalendarChange"
                    @rendermode="InteractiveServer" />
```

### Custom Add/Edit UI Example (`OnAddOrEditClick`)

When `OnAddOrEditClick` is assigned the built-in add/edit dialog is suppressed entirely. For a new event the callback receives a draft with `StartDate`/`EndDate` pre-filled from the clicked slot and an empty `Id`; for an edit it receives a clone of the existing event. Use `string.IsNullOrEmpty(ev.Id)` to distinguish create from edit.

```razor
<BlazorFullCalendar Events="myEvents"
                    OnAddOrEditClick="HandleAddOrEdit"
                    OnChange="HandleCalendarChange"
                    @rendermode="InteractiveServer" />

@code {
    private List<BlazorFullCalendarEvent> myEvents = new();

    private Task HandleAddOrEdit(BlazorFullCalendarEvent? ev)
    {
        if (ev is null) return Task.CompletedTask;

        if (string.IsNullOrEmpty(ev.Id))
        {
            // New event — open your own creation dialog
        }
        else
        {
            // Existing event — open your own edit dialog
        }

        return Task.CompletedTask;
    }

    private Task HandleCalendarChange(BlazorFullCalendarChangeEventArgs args)
        => Task.CompletedTask;
}
```

### Custom Event Click Example (`OnEventClick`)

When `OnEventClick` is assigned the built-in event details dialog is suppressed when any event is clicked (in all views). The callback receives the clicked `BlazorFullCalendarEvent` so you can show your own details UI.

```razor
<BlazorFullCalendar Events="myEvents"
                    OnEventClick="HandleEventClick"
                    OnChange="HandleCalendarChange"
                    @rendermode="InteractiveServer" />

@code {
    private List<BlazorFullCalendarEvent> myEvents = new();

    private Task HandleEventClick(BlazorFullCalendarEvent ev)
    {
        // Show your own event details dialog / side panel / navigation
        return Task.CompletedTask;
    }

    private Task HandleCalendarChange(BlazorFullCalendarChangeEventArgs args)
        => Task.CompletedTask;
}
```

### Localization Notes

- The event add/edit dialog uses a custom dropdown date-time picker instead of native browser date inputs.
- Date cells, weekday headers, month names, and year/day values are rendered from the active `CultureInfo` calendar.
- This improves consistency for non-Gregorian cultures such as Persian (`fa-IR`) and other localized calendars.
- Dialog labels and validation text can be localized by supplying a customized `BlazorFullCalendarTexts` instance.
- Use `CultureName` (a plain string) instead of `Culture` (a `CultureInfo`) when using `@rendermode="InteractiveServer"`, because `CultureInfo` is not JSON-serializable by Blazor's parameter persistence.

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

---

## Component Parameters

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `Events` | `List<BlazorFullCalendarEvent>?` | `null` | List of calendar events to display |
| `Culture` | `CultureInfo?` | `CultureInfo.CurrentUICulture` | Sets calendar/date rendering and formatting. Do not use with `@rendermode="InteractiveServer"` — use `CultureName` instead |
| `CultureName` | `string?` | `null` | Culture name shortcut (e.g. `"fa-IR"`, `"ar-SA"`, `"fr-FR"`). Takes precedence over `Culture` when both are supplied |
| `Texts` | `BlazorFullCalendarTexts` | `new()` | Custom UI strings for labels, placeholders, action buttons, aria labels, and validation messages |
| `Theme` | `BlazorFullCalendarTheme` | `Default` | Visual theme — `Default` or `Fluent` (WinUI-style). Dark mode is supported for both |
| `EventColorOptions` | `IReadOnlyList<BlazorFullCalendarColorOption>?` | `null` | Ordered list of event colors shown in pickers and filters. When `null` all colors are shown in enum order |
| `OnChange` | `EventCallback<BlazorFullCalendarChangeEventArgs>` | — | Raised when a user adds, edits, or deletes an event (`Kind`: `Add`, `Edit`, `Delete`; `Source`: `Dialog`, `Drag`, `Resize`, `Delete`) |
| `OnAddOrEditClick` | `EventCallback<BlazorFullCalendarEvent?>` | — | When assigned, the built-in add/edit dialog is suppressed. Receives a draft event (empty `Id` = create) or a clone of the existing event (edit). Show your own UI and update `Events` after persisting |
| `OnEventClick` | `EventCallback<BlazorFullCalendarEvent>` | — | When assigned, the built-in event details dialog is suppressed when an event is clicked. Receives the clicked event so you can show your own details UI |
| `LoadAssets` | `bool` | `true` | When `true` the component automatically injects its CSS and JS into the page on first render. Set to `false` to manage assets manually (see [Asset loading](#4-asset-loading)) |

---

## Models

### BlazorFullCalendarEvent

```csharp
public class BlazorFullCalendarEvent
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public BlazorFullCalendarEventColor Color { get; set; }
    public List<BlazorFullCalendarAttendee> Attendees { get; set; }

    // Computed
    public bool IsSingleDay { get; }    // StartDate.Date == EndDate.Date
    public bool IsMultiDay { get; }     // !IsSingleDay
    public TimeSpan Duration { get; }   // EndDate - StartDate
}
```

### BlazorFullCalendarAttendee

```csharp
public class BlazorFullCalendarAttendee
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? Id { get; set; }

    // Computed
    public string FullName { get; }   // "FirstName LastName"
    public string Initials { get; }   // e.g. "AJ"
}
```

### BlazorFullCalendarEventColor

Available values: `Blue`, `Green`, `Red`, `Yellow`, `Purple`, `Orange`

### BlazorFullCalendarChangeEventArgs

```csharp
public sealed class BlazorFullCalendarChangeEventArgs
{
    public required BlazorFullCalendarEvent Event { get; init; }   // new/updated state (or removed snapshot for Delete)
    public BlazorFullCalendarEvent? OldEvent { get; init; }        // previous state (Edit/Delete); null for Add
    public required BlazorFullCalendarChangeKind Kind { get; init; } // Add | Edit | Delete
    public BlazorFullCalendarChangeSource Source { get; init; }      // Dialog | Drag | Resize | Delete
}
```

---

## Views

- **Month View**: Grid layout with multi-day event support and "+N more" overflow
- **Week View**: 7-day view with hourly time slots, drag-and-drop, and event resize
- **Day View**: Single-day detailed view with timeline, sidebar mini-calendar, and "Happening Now" panel
- **Year View**: 12-month overview with per-day event bullet indicators
- **Agenda View**: Searchable list grouped by date or user

---

## Customization

The calendar includes built-in settings accessible via the gear icon in the header:

- Toggle dark mode
- Switch between 12/24-hour time format
- Choose badge style (colored or dot)
- Set start hour for day/week views
- Configure agenda view grouping

### CSS Customization

All CSS classes use the `bfc-` prefix (e.g. `bfc-root`, `bfc-header`, `bfc-btn`) and all CSS custom properties use `--bfc-` (e.g. `--bfc-primary`, `--bfc-bg`, `--bfc-border`). You can override any variable on `:root` or scope overrides to `.bfc-root`:

```css
.bfc-root {
    --bfc-primary: #8b5cf6;
    --bfc-radius: 12px;
}
```

Key CSS variables:

| Variable | Description |
|----------|-------------|
| `--bfc-bg` | Main background color |
| `--bfc-bg-secondary` | Secondary/subtle background |
| `--bfc-bg-hover` | Hover state background |
| `--bfc-border` | Border color |
| `--bfc-text` | Primary text color |
| `--bfc-text-secondary` | Secondary text color |
| `--bfc-text-muted` | Muted/disabled text color |
| `--bfc-primary` | Accent/brand color |
| `--bfc-primary-hover` | Accent hover state |
| `--bfc-primary-text` | Text on accent backgrounds |
| `--bfc-danger` | Destructive action color |
| `--bfc-radius` | Border radius for panels |
| `--bfc-radius-sm` | Border radius for small elements |
| `--bfc-shadow` | Default box shadow |
| `--bfc-shadow-lg` | Elevated box shadow |
| `--bfc-hour-height` | Height of one hour row in day/week views (default `96px`) |

Dark mode overrides are applied automatically via the `.bfc-dark` class.

---

## Static Asset Paths

When `LoadAssets="false"`, the files are served from the Razor Class Library's static web assets path:

| Asset | Path |
|-------|------|
| Stylesheet (base + Fluent theme) | `_content/BlazorFullCalendar/css/blazor-fullcalendar.css` |
| Fluent theme overrides only | `_content/BlazorFullCalendar/css/blazor-fullcalendar.fluent.css` |
| JavaScript interop | `_content/BlazorFullCalendar/js/blazor-fullcalendar.js` |

> `blazor-fullcalendar.css` already `@import`s `blazor-fullcalendar.fluent.css`, so you only need to reference the main stylesheet.

---

## Browser Support

- Modern browsers with CSS Grid and Flexbox support
- HTML5 Drag and Drop API support required for drag-and-drop

---

## Credits

Original concept inspired by [yassir-jeraidi/full-calendar](https://github.com/yassir-jeraidi/full-calendar)

## License

MIT, use it and be happy :)
