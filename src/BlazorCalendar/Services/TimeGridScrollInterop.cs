using Microsoft.JSInterop;

namespace BlazorCalendar;

internal static class TimeGridScrollInterop
{
    public static async ValueTask<bool> TryScrollToStartOfDayAsync(
        IJSRuntime js,
        string elementId,
        int startOfDayHour,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await js.InvokeVoidAsync(
                "calendarInterop.scrollToHour",
                cancellationToken,
                elementId,
                startOfDayHour,
                BlazorCalendarHelpers.HourHeightPx);
            return true;
        }
        catch (JSDisconnectedException)
        {
            return false;
        }
        catch (OperationCanceledException)
        {
            return false;
        }
        catch (JSException)
        {
            return false;
        }
        catch (InvalidOperationException)
        {
            return false;
        }
    }
}
