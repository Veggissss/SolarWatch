using System.ComponentModel.DataAnnotations;

namespace SolarWatch.Models;

public class User
{
    [MaxLength(32)]
    public required string Username { get; set; }
    [MaxLength(128)]
    public required string Password { get; set; }
}