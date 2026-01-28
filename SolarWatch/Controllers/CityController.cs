using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolarWatch.Models;
using SolarWatch.Repositories;

namespace SolarWatch.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CityController(ICityRepository cityRepository) : ControllerBase
{
    [Authorize(Roles = "Admin, User")]
    [HttpGet]
    public async Task<IActionResult> GetCities()
    {
        return Ok(await new Task<List<City>>(() => cityRepository.ReadAll().ToList()));
    }
    
    [Authorize(Roles = "Admin, User")]
    [HttpGet]
    public async Task<IActionResult> GetCity(int id)
    {
        var city = await new Task<City?>(() => cityRepository.Read(id));
        if (city == null)
        {
            return NotFound("City not found");
        }
        return Ok(city);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut]
    public async Task<IActionResult> UpdateCity([FromBody] City city)
    {
        await new Task(() => cityRepository.Update(city));
        return Ok("Updated City");
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> AddCity([FromBody] City city)
    {
        await new Task(() => cityRepository.Create(city));
        return Ok("Added City");
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete]
    public async Task<IActionResult> DeleteCity(int id)
    {
        await new Task(() => cityRepository.Delete(id));
        return Ok("Deleted City");
    }
}