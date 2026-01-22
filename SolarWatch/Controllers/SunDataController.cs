using Microsoft.AspNetCore.Mvc;
using SolarWatch.DTOs;
using SolarWatch.Repositories;
using SolarWatch.Services;

namespace SolarWatch.Controllers;

[ApiController]
[Route("[controller]")]
public class SunDataController(
    IDateService dateService,
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

        var savedCity = await cityRepository.GetByName(city);
        if (savedCity == null)
        {
            return Problem("City not found", statusCode: 404);
        }

        var savedSunData = await sunDataRepository.GetByCityAndDate(savedCity, date);
        if (savedSunData == null)
        {
            return Problem("Invalid sunset response", statusCode: 500);
        }

        return Ok(new SunDataDTO(savedSunData.Sunrise, savedSunData.Sunset));
    }
}