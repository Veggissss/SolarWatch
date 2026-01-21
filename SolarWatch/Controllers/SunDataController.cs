using System.Globalization;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using SolarWatch.DTOs;
using SolarWatch.Models;
using SolarWatch.Repositories;

namespace SolarWatch.Controllers;

[ApiController]
[Route("[controller]")]
public class SunDataController(
    IConfiguration config,
    IHttpClientFactory httpClientFactory,
    ISunDataRepository sunDataRepository,
    ICityRepository cityRepository)
    : ControllerBase
{
    private readonly HttpClient _http = httpClientFactory.CreateClient();
    private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

    [HttpGet]
    public async Task<IActionResult> Get(string city, string? date)
    {
        if (date != null && !IsValidDate(date))
        {
            return Problem("Invalid date", statusCode: 400);
        }

        var savedCity = cityRepository.ReadAll().FirstOrDefault(cityObj => cityObj.Name == city);
        if (savedCity == null)
        {
            var cityLocation = await GetCityCoordinates(city);
            if (cityLocation == null)
            {
                return Problem("City not found", statusCode: 404);
            }

            savedCity = new City(cityLocation.Name, cityLocation.Lat, cityLocation.Lon, cityLocation.State,
                cityLocation.Country);
            cityRepository.Create(savedCity);
        }

        var savedSunData = sunDataRepository.ReadAll()
            .FirstOrDefault(savedSunData => savedSunData.City.Name == city && savedSunData.Date == date);
        if (savedSunData == null)
        {
            var sunData = await GetSunData(savedCity.Latitude, savedCity.Longitude, date);
            if (sunData == null)
            {
                return Problem("Invalid sunset response", statusCode: 500);
            }

            savedSunData = new SunData(sunData.Sunrise, sunData.Sunset, date ?? GetDateToday(), savedCity);
            sunDataRepository.Create(savedSunData);
        }

        savedCity.SunData.Add(savedSunData);
        cityRepository.Update(savedCity);

        return Ok(new SunDataDTO(savedSunData.Sunrise, savedSunData.Sunset));
    }

    private async Task<OpenWeatherDTO?> GetCityCoordinates(string city)
    {
        var apiKey = config["OPENWEATHERMAP_API"];
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            return null;
        }

        var cityLocationUrl = $"http://api.openweathermap.org/geo/1.0/direct?q={city}&appid={apiKey}";
        var cityLocationResponse = await _http.GetStringAsync(cityLocationUrl);
        var cityLocations = JsonSerializer.Deserialize<OpenWeatherDTO[]>(cityLocationResponse, _jsonOptions);

        return cityLocations != null && cityLocations.Length > 0 ? cityLocations[0] : null;
    }

    private async Task<SunDataDTO?> GetSunData(float lat, float lon, string? date)
    {
        var sunsetUrl = $"https://api.sunrise-sunset.org/json?lat={lat}&lng={lon}&date={date}";
        var sunsetResponse = await _http.GetStringAsync(sunsetUrl);
        var sunsetData = JsonSerializer.Deserialize<SunDataResponseDTO>(sunsetResponse, _jsonOptions);

        return sunsetData?.Results;
    }

    private bool IsValidDate(string input)
    {
        return DateTime.TryParseExact(
            input,
            "yyyy-MM-dd",
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out _
        );
    }

    private string GetDateToday()
    {
        return DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
    }
}