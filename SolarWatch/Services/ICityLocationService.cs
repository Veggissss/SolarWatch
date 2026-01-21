using SolarWatch.DTOs;

namespace SolarWatch.Services;

public interface ICityLocationService
{
    Task<CityLocationDTO?> GetCityLocation(string cityName);
}