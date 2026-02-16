using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolarWatch.DTOs;
using SolarWatch.Models;
using SolarWatch.Repositories;
using SolarWatch.Services;

namespace SolarWatch.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SunDataController(
    IDateService dateService,
    ISunDataRepository sunDataRepository,
    ICityRepository cityRepository)
    : ControllerBase
{
    [Authorize(Roles = "User,Admin")]
    [HttpGet]
    public async Task<IActionResult> Get(string city, string? date)
    {
        city = string.Join(" ", city.ToLower().Split(' ').Select(word => char.ToUpper(word[0]) + word[1..]));
        
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

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] SunDataDTO sunDataDto, string city, string date)
    {
        var savedCity = await cityRepository.GetByName(city);
        if (savedCity == null)
        {
            return NotFound("City not found");
        }
        sunDataRepository.Create(new SunData(sunDataDto.Sunrise, sunDataDto.Sunset, date, savedCity));
        return Ok("Created sun data");
    }

    [Authorize(Roles = "Admin")]
    [HttpPut]
    public async Task<IActionResult> Put(int id, [FromBody] SunDataDTO sunDataDto, string city, string date)
    {
        var savedCity =  await cityRepository.GetByName(city);
        if (savedCity == null)
        {
            return NotFound("City not found");
        }
        
        var savedSunData = sunDataRepository.Read(id);
        if (savedSunData == null)
        {
            return NotFound("Sun data not found");
        }
        savedSunData.Sunrise = sunDataDto.Sunrise;
        savedSunData.Sunset = sunDataDto.Sunset;
        savedSunData.CityId = savedCity.Id;
        savedSunData.City = savedCity;
        savedSunData.Date = date;
        
        sunDataRepository.Update(savedSunData);
        return Ok("Updated sun data");
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete]
    public async Task<IActionResult> Delete(string city, string date)
    {
        var storedCity = await Task.Run(() => cityRepository.ReadAll().FirstOrDefault(c => c.Name == city));
        if (storedCity == null)
        {
            return NotFound("City not found");
        }

        var storedSunData = await Task.Run(() => sunDataRepository.ReadAll()
            .FirstOrDefault(sunData => sunData.CityId == storedCity.Id && sunData.Date == date));
        if (storedSunData == null)
        {
            return NotFound("Sun data with specified city and date not found");
        }

        await Task.Run(() => sunDataRepository.Delete(storedSunData.Id));
        return Ok("Deleted sun data");
    }
}