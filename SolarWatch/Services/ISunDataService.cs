using SolarWatch.DTOs;

namespace SolarWatch.Services;

public interface ISunDataService
{
    Task<SunDataDTO?> GetSunData(float lat, float lon, string? date);
}