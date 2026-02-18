using Microsoft.EntityFrameworkCore;
using SolarWatch.Configuration;

namespace SolarWatch;

public abstract class AbstractDbContext(IConfiguration config, DbContextOptions options) : DbContext(options)
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured)
        {
            return;
        }
        var databaseSettings = config.GetSection("Database").Get<DatabaseSettings>() ??
                               throw new Exception("Missing environment variable sections Database__*");
        optionsBuilder.UseNpgsql(
            $"Host={databaseSettings.Host};Port={databaseSettings.Port};Username={databaseSettings.Username};Password={databaseSettings.Password};Database={databaseSettings.Name}"
        );
    }
}