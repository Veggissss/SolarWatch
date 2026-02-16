using SolarWatch.DTOs;

namespace SolarWatch.DAOs;

public interface ISunDataDao
{
    Task<SunDataDTO?> GetSunData(float lat, float lon, string? date);
}