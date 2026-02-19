using SolarWatch.DAOs;
using SolarWatch.Models;
using SolarWatch.Services;

namespace SolarWatch.Repositories;

public class SunDataRepository(SolarWatchDbContext context, ISunDataDao sunDataDao, IDateService dateService)
    : ISunDataRepository
{
    public void Create(SunData entity)
    {
        context.Add(entity);
        context.SaveChanges();
    }

    public IEnumerable<SunData> ReadAll()
    {
        return context.SunDatas;
    }

    public SunData? Read(int id)
    {
        return context.SunDatas.Find(id);
    }

    public void Update(SunData entity)
    {
        context.Update(entity);
        context.SaveChanges();
    }

    public void Delete(int id)
    {
        var sunData = context.SunDatas.Find(id);
        if (sunData == null)
        {
            return;
        }

        context.Remove(sunData);
        context.SaveChanges();
    }

    public async Task<SunData?> GetByCityAndDate(City city, string? date)
    {
        var queryDate = date ?? dateService.GetDateToday();
        var savedSunData = ReadAll().FirstOrDefault(savedSunData =>
            savedSunData.CityId == city.Id && queryDate == savedSunData.Date
        );
        if (savedSunData != null)
        {
            return savedSunData;
        }

        var sunData = await sunDataDao.GetSunData(city.Latitude, city.Longitude, date);
        if (sunData == null)
        {
            return null;
        }

        savedSunData = new SunData(sunData.Sunrise, sunData.Sunset, date ?? dateService.GetDateToday(), city);
        Create(savedSunData);
        return savedSunData;
    }
}