using Microsoft.EntityFrameworkCore;
using SolarWatch.Models;

namespace SolarWatch;

public class SolarWatchDbContext(IConfiguration config, DbContextOptions<SolarWatchDbContext> options) : AbstractDbContext(config, options)
{
    public DbSet<City> Cities { get; set; }
    public DbSet<SunData> SunDatas { get; set; }
    public DbSet<User> Users { get; set; }
}