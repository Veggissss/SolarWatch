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

    [HttpGet]
    public async Task<IActionResult> Get(string city, string? date)
    {
        if (date != null && !IsValidDate(date))
        {
            return Problem("Invalid date", statusCode: 400);
        }
        
        var apiKey = config["OPENWEATHERMAP_API"];
        if (string.IsNullOrWhiteSpace(apiKey))
            return Problem("Missing OPENWEATHERMAP_API configuration value!", statusCode: 500);

        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var cityLocationUrl = $"http://api.openweathermap.org/geo/1.0/direct?q={city}&appid={apiKey}";
        var cityLocationResponse = await _http.GetStringAsync(cityLocationUrl);
        var cityLocations = JsonSerializer.Deserialize<OpenWeatherDTO[]>(cityLocationResponse, jsonOptions);
        if (cityLocations == null || cityLocations.Length == 0)
        {
            return Problem("City not found", statusCode: 404);
        }

        var cityLocation = cityLocations[0];

        var sunsetUrl =
            $"https://api.sunrise-sunset.org/json?lat={cityLocation.Lat}&lng={cityLocation.Lon}&date={date}";
        var sunsetResponse = await _http.GetStringAsync(sunsetUrl);
        var sunsetData = JsonSerializer.Deserialize<SunDataResponseDTO>(sunsetResponse, jsonOptions);

        return sunsetData?.Results == null
            ? Problem("Invalid sunset response", statusCode: 500)
            : Ok(sunsetData.Results);
    }


    bool IsValidDate(string input)
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