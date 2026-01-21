using System.Text.Json;
using SolarWatch.DTOs;

namespace SolarWatch.Services;

public class SunDataService(IHttpClientFactory httpClientFactory) : ISunDataService
{
    private readonly HttpClient _http = httpClientFactory.CreateClient();
    private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };
    
    public async Task<SunDataDTO?> GetSunData(float lat, float lon, string? date)
    {
        var sunsetUrl = $"https://api.sunrise-sunset.org/json?lat={lat}&lng={lon}&date={date}";
        var sunsetResponse = await _http.GetStringAsync(sunsetUrl);
        var sunsetData = JsonSerializer.Deserialize<SunDataResponseDTO>(sunsetResponse, _jsonOptions);

        return sunsetData?.Results;
    }
}