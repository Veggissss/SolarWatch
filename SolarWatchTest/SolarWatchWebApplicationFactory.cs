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
                services.SingleOrDefault(d => d.ServiceType == typeof(SunSetContext));
            var userDbContextDescriptor =
                services.SingleOrDefault(d => d.ServiceType == typeof(UserContext));
            if (solarWatchDbContextDescriptor == null || userDbContextDescriptor == null)
            {
                throw new Exception("The DbContext was not found!");
            }

            //Remove the previous DbContextOptions registration
            services.Remove(solarWatchDbContextDescriptor);
            services.Remove(userDbContextDescriptor);

            services.AddDbContext<SunSetContext>(options => { options.UseInMemoryDatabase(_dbName); });
            services.AddDbContext<UserContext>(options => { options.UseInMemoryDatabase(_dbName); });

            //Since DbContexts are scoped services, we create a scope
            using var scope = services.BuildServiceProvider().CreateScope();

            //We use this scope to request the registered dbcontext, and initialize the schemas
            var solarContext = scope.ServiceProvider.GetRequiredService<SunSetContext>();
            var userContext = scope.ServiceProvider.GetRequiredService<UserContext>();
            solarContext.Database.EnsureDeleted();
            userContext.Database.EnsureDeleted();
            solarContext.Database.EnsureCreated();
            userContext.Database.EnsureCreated();

            //Here we could do more initializing if we wished (e.g. adding admin user)
            var passwordHasher = scope.ServiceProvider.GetRequiredService<PasswordHasher>();
            var hashedPassword = passwordHasher.HashPassword("password");
            userContext.Users.Add(new User(new LoginDTO("admin", hashedPassword)));
            userContext.SaveChanges();
        });
    }
}