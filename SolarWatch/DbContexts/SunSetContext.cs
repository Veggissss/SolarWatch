using Microsoft.EntityFrameworkCore;
using SolarWatch.Models;

namespace SolarWatch;

public class SunSetContext(IConfiguration config) : AbstractDbContext(config)
{
    public DbSet<City> Cities { get; set; }
    public DbSet<SunData> SunDatas { get; set; }
}