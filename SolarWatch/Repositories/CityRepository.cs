using SolarWatch.DAOs;
using SolarWatch.Models;

namespace SolarWatch.Repositories;

public class CityRepository(SunSetContext context, ICityLocationDao cityLocationDao)
    : ICityRepository
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

    public City? Read(int id)
    {
        return context.Cities.Find(id);
    }

    public async Task<City?> GetByName(string name)
    {
        var savedCity = ReadAll()
            .FirstOrDefault(cityObj => string.Equals(cityObj.Name, name, StringComparison.OrdinalIgnoreCase));
        if (savedCity != null)
        {
            return savedCity;
        }

        var cityLocation = await cityLocationDao.GetCityLocation(name);
        if (cityLocation == null)
        {
            return null;
        }

        savedCity = new City(cityLocation.Name.Trim(),
            cityLocation.Lat, cityLocation.Lon,
            cityLocation.State, cityLocation.Country
        );
        Create(savedCity);
        return savedCity;
    }
}