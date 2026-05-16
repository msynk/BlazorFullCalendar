using Microsoft.JSInterop;

namespace BlazorFullCalendar;

internal static class AgendaScrollInterop
{
    public static async ValueTask<bool> TryScrollToDateAsync(
        IJSRuntime js,
        string scrollContainerId,
        DateTime date,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await js.InvokeVoidAsync(
                "BlazorFullCalendar.scrollAgendaToDate",
                cancellationToken,
                scrollContainerId,
                date.ToString("yyyy-MM-dd"));
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
