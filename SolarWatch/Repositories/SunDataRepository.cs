using SolarWatch.Models;

namespace SolarWatch.Repositories;

public class SunDataRepository(SunSetContext context) : ISunDataRepository
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
}