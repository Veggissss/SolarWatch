using System.ComponentModel.DataAnnotations;

namespace SolarWatch.Models;

public class City
{
    public int Id { get; set; }
    [MaxLength(200)] public string Name { get; set; }
    public float Latitude { get; set; }
    public float Longitude { get; set; }

    [MaxLength(200)] public string Country { get; set; }
    [MaxLength(200)] public string? State { get; set; }

    public List<SunData> SunData { get; }

    public City()
    {
        Name = string.Empty;
        Country = string.Empty;
        SunData = [];
    }

    public City(string name, float latitude, float longitude, string? state, string country)
    {
        Name = name;
        Longitude = longitude;
        Latitude = latitude;
        State = state;
        Country = country;
        SunData = [];
    }
}