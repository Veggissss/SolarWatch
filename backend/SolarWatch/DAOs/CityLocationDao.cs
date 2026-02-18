using System.Text.Json;
using SolarWatch.Configuration;
using SolarWatch.DTOs;

namespace SolarWatch.DAOs;

public class CityLocationDao(
    IHttpClientFactory httpClientFactory,
    IConfiguration config,
    ILogger<CityLocationDao> logger) : ICityLocationDao
{
    private readonly HttpClient _http = httpClientFactory.CreateClient();
    private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

    public async Task<CityLocationDTO?> GetCityLocation(string cityName)
    {
        var apiKey = config.GetSection("ExternalApiKeys").GetSection("OpenWeatherMap").Value ?? throw new MissingMemberException("ExternalApiKeys__OpenWeatherMap is missing!");
        var cityLocationUrl = $"http://api.openweathermap.org/geo/1.0/direct?q={cityName}&appid={apiKey}";
        var response = await _http.GetAsync(cityLocationUrl);

        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            logger.LogWarning("Invalid OpenWeatherMap API key!");
            return null;
        }

        response.EnsureSuccessStatusCode();
        var cityLocationResponse = await response.Content.ReadAsStringAsync();
        var cityLocations = JsonSerializer.Deserialize<CityLocationDTO[]>(cityLocationResponse, _jsonOptions);
        if (cityLocations == null || cityLocations.Length == 0)
        {
            logger.LogInformation("No city locations found for {CityName}.", cityName);
            return null;
        }
        var city = cityLocations[0];
        city.Name = cityName;
        return city;
    }
}