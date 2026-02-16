namespace SolarWatch.DTOs;

public class SunDataDTO(string sunrise, string sunset)
{
    public string Sunrise { get; private set; } = sunrise;
    public string Sunset { get; private set; } = sunset;
}