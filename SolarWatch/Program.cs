using dotenv.net;
using SolarWatch;
using SolarWatch.DAOs;
using SolarWatch.Repositories;
using SolarWatch.Services;

// Load API keys
DotEnv.Load();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddScoped<SunSetContext>();

builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<ISunDataRepository, SunDataRepository>();

builder.Services.AddScoped<IDateService, DateService>();
builder.Services.AddScoped<ICityLocationDao, CityLocationDao>();
builder.Services.AddScoped<ISunDataDao, SunDataDao>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();