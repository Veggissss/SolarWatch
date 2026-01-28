using SolarWatch.DTOs;
using SolarWatch.Services;
using Microsoft.AspNetCore.Mvc;
using SolarWatch.Models;
using SolarWatch.Repositories;

namespace SolarWatch.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(JwtTokenService tokens, IUserRepository userRepository, PasswordHasher passwordHasher) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO req)
    {
        var user = await Task.Run(() =>
            userRepository.ReadAll().FirstOrDefault(x => x.Username == req.Username));
        
        if (user == null || !passwordHasher.VerifyPassword(req.Password, user.Password))
        {
            return Unauthorized();
        }

        var token = tokens.CreateToken(user.Id.ToString(), req.Username, ["User"]);
        Response.Cookies.Append("Authorization", token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddHours(JwtTokenService.JwtValidMinutes)
        });

        return Ok();
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] LoginDTO req)
    {
        var userExists = await Task.Run(() => userRepository.ReadAll().Any(x => x.Username == req.Username));
        if (userExists)
        {
            return Unauthorized("Username already in use");
        }

        var hashedPassword = passwordHasher.HashPassword(req.Password);
        var newUser = new User(req) { Password = hashedPassword };
        await Task.Run(() => userRepository.Create(newUser));
        return Ok();
    }
}