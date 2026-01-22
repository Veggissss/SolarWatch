using SolarWatch.Models;

namespace SolarWatch.Repositories;

public interface ISunDataRepository : IRepository<SunData, int>
{
    Task<SunData?> GetByCityAndDate (City city, string? date);
}