namespace SolarWatch.Models;

public class SunData
{
    public int Id { get; set; }
    public string Sunrise { get; set; }
    public string Sunset { get; set; }
    public string Date { get; set; }

    public int CityId { get; set; }
    public City City { get; set; }

    public SunData()
    {
    }

    public SunData(string sunrise, string sunset, string date, City city)
    {
        Sunrise = sunrise;
        Sunset = sunset;
        Date = date;
        City = city;
    }
}