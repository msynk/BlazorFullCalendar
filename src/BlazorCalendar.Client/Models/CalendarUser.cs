namespace BlazorCalendar.Client.Models;

public class CalendarUser
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? PicturePath { get; set; }

    public string Initials
    {
        get
        {
            if (string.IsNullOrWhiteSpace(Name)) return "";
            var words = Name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (words.Length == 1) return words[0][..1].ToUpperInvariant();
            return $"{char.ToUpperInvariant(words[0][0])}{char.ToUpperInvariant(words[1][0])}";
        }
    }
}
