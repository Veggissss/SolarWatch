namespace SolarWatch.Services;

public interface IDateService
{
    bool IsValidDate(string input);

    string GetDateToday();
}