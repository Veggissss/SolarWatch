using SolarWatch.DTOs;

namespace SolarWatch.DAOs;

public interface ICityLocationDao
{
    Task<CityLocationDTO?> GetCityLocation(string cityName);
}