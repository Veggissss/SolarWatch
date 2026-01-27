using Microsoft.EntityFrameworkCore;

namespace SolarWatch;

abstract public class AbstractDbContext(IConfiguration config) : DbContext
{
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var databaseName = config["DB_NAME"] ?? throw new Exception("Missing environment variable DB_NAME");
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