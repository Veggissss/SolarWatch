using Microsoft.EntityFrameworkCore;

namespace SolarWatch;

public abstract class AbstractDbContext(IConfiguration config, DbContextOptions options) : DbContext(options)
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured)
        {
            return;
        }

        var databaseName = config["DB_NAME"] ?? throw new Exception("Missing environment variable DB_NAME");

        // Skip Postgres configuration if running in test mode (database name is a GUID)
        if (!string.IsNullOrEmpty(databaseName) && Guid.TryParse(databaseName, out _))
        {
            return;
        }

        var databaseUsername =
            config["DB_USERNAME"] ?? throw new Exception("Missing environment variable DB_USERNAME");
        var databasePassword =
            config["DB_PASSWORD"] ?? throw new Exception("Missing environment variable DB_PASSWORD");
        var databaseHost = config["DB_HOST"] ?? "localhost";
        var databasePort = config["DB_PORT"] ?? "5432";
        optionsBuilder.UseNpgsql(
            $"Host={databaseHost};Port={databasePort};Username={databaseUsername};Password={databasePassword};Database={databaseName}");
    }
}