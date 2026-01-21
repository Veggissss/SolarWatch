using System.Globalization;

namespace SolarWatch.Services;

public class DateService : IDateService
{
    public bool IsValidDate(string input)
    {
        return DateTime.TryParseExact(
            input,
            "yyyy-MM-dd",
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out _
        );
    }

    public string GetDateToday()
    {
        return DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
    }
}