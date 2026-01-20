using System.Globalization;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using SolarWatch.DTOs;

namespace SolarWatch.Controllers;

[ApiController]
[Route("[controller]")]
public class SunDataController(IConfiguration config, IHttpClientFactory httpClientFactory)
    : ControllerBase
{
    private readonly HttpClient _http = httpClientFactory.CreateClient();
    private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions(){PropertyNameCaseInsensitive = true};

    [HttpGet]
    public async Task<IActionResult> Get(string city, string? date)
    {
        if (date != null && !IsValidDate(date))
        {
            return Problem("Invalid date", statusCode: 400);
        }

        var cityLocation = await GetCityCoordinates(city);
        if (cityLocation == null)
        {
            return Problem("City not found", statusCode: 404);
        }

        var sunData = await GetSunData(cityLocation.Lat, cityLocation.Lon, date);
        if (sunData == null)
        {
            return Problem("Invalid sunset response", statusCode: 500);
        }

        return Ok(sunData);
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
            "yyyy-mm-dd",
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out _
        );
    }
}