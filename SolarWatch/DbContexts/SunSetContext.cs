using Microsoft.EntityFrameworkCore;
using SolarWatch.Models;

namespace SolarWatch;

public class SunSetContext(IConfiguration config, DbContextOptions<SunSetContext> options) : AbstractDbContext(config, options)
{
    public DbSet<City> Cities { get; set; }
    public DbSet<SunData> SunDatas { get; set; }
}