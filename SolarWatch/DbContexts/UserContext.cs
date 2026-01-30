using Microsoft.EntityFrameworkCore;
using SolarWatch.Models;

namespace SolarWatch;

public class UserContext(IConfiguration config, DbContextOptions<UserContext> options) : AbstractDbContext(config, options)
{
    public DbSet<User> Users { get; set; }
}