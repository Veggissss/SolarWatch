using System.ComponentModel.DataAnnotations;
using SolarWatch.DTOs;

namespace SolarWatch.Models;

public class User
{
    public int Id { get; set; }
    [MaxLength(32)] public string Username { get; set; }
    [MaxLength(128)] public string Password { get; set; }

    public User(LoginDTO login)
    {
        Username = login.Username;
        Password = login.Password;
    }


    public User()
    {
        
    }
}