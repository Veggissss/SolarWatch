using SolarWatch.Models;

namespace SolarWatch.Repositories;

public class CityRepository(SunSetContext context) : ICityRepository
{
    public void Create(City entity)
    {
        context.Add(entity);
        context.SaveChanges();
    }

    public IEnumerable<City> ReadAll()
    {
        return context.Cities;
    }

    public City? Read(int id)
    {
        return context.Cities.Find(id);
    }

    public void Update(City entity)
    {
        context.Update(entity);
        context.SaveChanges();
    }

    public void Delete(int id)
    {
        var city = Read(id);
        if (city == null)
        {
            return;
        }
        context.Cities.Remove(city);
        context.SaveChanges();
    }
}