using Microsoft.AspNetCore.Mvc;
using SolarWatch.DTOs;
using SolarWatch.Models;
using SolarWatch.Repositories;
using SolarWatch.Services;

namespace SolarWatch.Controllers;

[ApiController]
[Route("[controller]")]
public class SunDataController(
    IDateService dateService,
    ICityLocationService cityLocationService,
    ISunDataService sunDataService,
    ISunDataRepository sunDataRepository,
    ICityRepository cityRepository)
    : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get(string city, string? date)
    {
        if (date != null && !dateService.IsValidDate(date))
        {
            return Problem("Invalid date", statusCode: 400);
        }

        var savedCity = cityRepository.ReadAll().FirstOrDefault(cityObj => cityObj.Name == city);
        if (savedCity == null)
        {
            var cityLocation = await cityLocationService.GetCityLocation(city);
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
            var sunData = await sunDataService.GetSunData(savedCity.Latitude, savedCity.Longitude, date);
            if (sunData == null)
            {
                return Problem("Invalid sunset response", statusCode: 500);
            }

            savedSunData = new SunData(sunData.Sunrise, sunData.Sunset, date ?? dateService.GetDateToday(), savedCity);
            sunDataRepository.Create(savedSunData);
        }

        savedCity.SunData.Add(savedSunData);
        cityRepository.Update(savedCity);

        return Ok(new SunDataDTO(savedSunData.Sunrise, savedSunData.Sunset));
    }
}