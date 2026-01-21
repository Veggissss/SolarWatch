namespace SolarWatch.Models;

public class City
{
    private string Name { get; }
    private float Longitude { get; }
    private float Latitude { get; }
    private string State { get; }
    private string Country { get; }

    private List<SunData> SunData { get; }

    public City(string name, float longitude, float latitude, string state, string country, List<SunData> sunData)
    {
        Name = name;
        Longitude = longitude;
        Latitude = latitude;
        State = state;
        Country = country;
        SunData = sunData;
    }
}