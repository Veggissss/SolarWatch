using SolarWatch.Models;

namespace SolarWatch.Repositories;

public interface ICityRepository : IRepository<City, int>
{
    Task<City?> GetByName(string name);
}