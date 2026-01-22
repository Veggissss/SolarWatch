using System.Text.Json;
using SolarWatch.DTOs;

namespace SolarWatch.DAOs;

public class CityLocationDao(IHttpClientFactory httpClientFactory, IConfiguration config) : ICityLocationDao
{
    private readonly HttpClient _http = httpClientFactory.CreateClient();
    private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };
    
    public async Task<CityLocationDTO?> GetCityLocation(string cityName)
    {
        var apiKey = config["OPENWEATHERMAP_API"];
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            return null;
        }

        var cityLocationUrl = $"http://api.openweathermap.org/geo/1.0/direct?q={cityName}&appid={apiKey}";
        var cityLocationResponse = await _http.GetStringAsync(cityLocationUrl);
        var cityLocations = JsonSerializer.Deserialize<CityLocationDTO[]>(cityLocationResponse, _jsonOptions);

        return cityLocations != null && cityLocations.Length > 0 ? cityLocations[0] : null;
    }
}