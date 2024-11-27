
using System.ComponentModel.DataAnnotations;

namespace MyAirbnb.Models.DTOs.Authentication;

public class AuthenticationRequestDTO
{
    [Required(ErrorMessage = "Please provide user name")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Please provide password")]
    public string Password { get; set; }
}
