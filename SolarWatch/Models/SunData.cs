namespace SolarWatch.Models;

public class SunData
{
    private string Sunrise { get; }
    private string Sunset { get; }

    private City City { get; }
    
    public SunData(string sunrise, string sunset, City city)
    {
        Sunrise = sunrise;
        Sunset = sunset;
        City = city;
    }
}