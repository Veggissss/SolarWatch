namespace SolarWatch.Models;

public class City
{
    private string _name;
    private float _longitude;
    private float _latitude;
    private string _state;
    private string _country;

    public City(string name, float longitude, float latitude, string state, string country)
    {
        _name = name;
        _longitude = longitude;
        _latitude = latitude;
        _state = state;
        _country = country;
    }
}