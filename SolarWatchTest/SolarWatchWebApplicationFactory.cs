using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SolarWatch;
using SolarWatch.DTOs;
using SolarWatch.Models;
using SolarWatch.Services;

namespace SolarWatchTest;

public class SolarWatchWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string _dbName = Guid.NewGuid().ToString();

    public SolarWatchWebApplicationFactory()
    {
        Environment.SetEnvironmentVariable("DB_NAME", _dbName);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            //Get the previous DbContext registration 
            var solarWatchDbContextDescriptor =
                services.SingleOrDefault(d => d.ServiceType == typeof(SolarWatchDbContext));
            if (solarWatchDbContextDescriptor == null)
            {
                throw new Exception("The DbContext was not found!");
            }

            //Remove the previous DbContextOptions registration
            services.Remove(solarWatchDbContextDescriptor);

            services.AddDbContext<SolarWatchDbContext>(options => { options.UseInMemoryDatabase(_dbName); });

            //Since DbContexts are scoped services, we create a scope
            using var scope = services.BuildServiceProvider().CreateScope();

            //We use this scope to request the registered dbcontext, and initialize the schemas
            var solarContext = scope.ServiceProvider.GetRequiredService<SolarWatchDbContext>();
            solarContext.Database.EnsureDeleted();
            solarContext.Database.EnsureCreated();

            //Here we could do more initializing if we wished (e.g. adding admin user)
            var passwordHasher = scope.ServiceProvider.GetRequiredService<PasswordHasher>();
            var hashedPassword = passwordHasher.HashPassword("password");
            solarContext.Users.Add(new User(new LoginDTO("admin", hashedPassword)));
            solarContext.SaveChanges();
        });
    }
}