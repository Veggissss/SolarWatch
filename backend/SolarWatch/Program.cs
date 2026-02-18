using System.Text;
using dotenv.net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SolarWatch;
using SolarWatch.Configuration;
using SolarWatch.DAOs;
using SolarWatch.DTOs;
using SolarWatch.Models;
using SolarWatch.Repositories;
using SolarWatch.Services;

DotEnv.Load();

var builder = WebApplication.CreateBuilder(args);

// Create JWT settings from configuration
var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>()
                  ?? throw new InvalidOperationException("JWT settings not found in configuration");

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
var key = Encoding.ASCII.GetBytes(jwtSettings.Secret);

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(key),
        };
    });


builder.Services.AddAuthorizationBuilder()
    .AddDefaultPolicy("User", new AuthorizationPolicyBuilder()
        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
        .RequireAuthenticatedUser()
        .Build())
    .AddPolicy("Admin", new AuthorizationPolicyBuilder()
        .RequireRole("Admin")
        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
        .RequireAuthenticatedUser()
        .Build());

// Add services to the container.
builder.Services.AddHttpClient();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<SolarWatchDbContext>();
builder.Services.AddScoped<PasswordHasher>();

builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<ISunDataRepository, SunDataRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<JwtTokenService>();
builder.Services.AddScoped<IDateService, DateService>();
builder.Services.AddScoped<ICityLocationDao, CityLocationDao>();
builder.Services.AddScoped<ISunDataDao, SunDataDao>();

var app = builder.Build();
var logger = app.Logger;

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<SolarWatchDbContext>();
    try
    {
        if (dbContext.Database.IsRelational())
        {
            logger.LogInformation("Applying database migrations...");
            dbContext.Database.Migrate();
            logger.LogInformation("Database migrations applied successfully.");
        }
        else
        {
            logger.LogWarning("Database is not relational.");
        }
        var adminUsername = builder.Configuration["Admin__Username"];
        if (!string.IsNullOrWhiteSpace(adminUsername)
            && !dbContext.Users.Any(user => user.Username == adminUsername))
        {
            logger.LogInformation("Creating admin account.");
            var passwordHasher = scope.ServiceProvider.GetRequiredService<PasswordHasher>();
            var adminPassword = builder.Configuration["Admin__Password"]
                                ?? throw new MissingMemberException("Missing Admin__Password env var.");
            var hashedPassword = passwordHasher.HashPassword(adminPassword);
            dbContext.Users.Add(new User(new LoginDTO(adminUsername, hashedPassword)) { IsAdmin = true });
            dbContext.SaveChanges();
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while migrating or seeding the database.");
        throw;
    }
}

//app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//app.UseHttpsRedirection();

app.MapControllers();

app.Run();