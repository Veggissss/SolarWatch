namespace SolarWatch.DTOs;

public class SunDataDTO
{
    public string Sunrise { get; private set; }
    public string Sunset { get; private set; }
    
    public SunDataDTO(string sunrise, string sunset)
    {
        Sunrise = sunrise;
        Sunset = sunset;
    }
}