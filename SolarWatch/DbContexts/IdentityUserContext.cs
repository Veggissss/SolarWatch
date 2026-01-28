using Microsoft.EntityFrameworkCore;
using SolarWatch.Models;

namespace SolarWatch;

public class IdentityUserContext(IConfiguration config) : AbstractDbContext(config)
{
    public DbSet<User> Users { get; set; }
}