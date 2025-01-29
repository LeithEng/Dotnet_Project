using System.ComponentModel.DataAnnotations;
namespace api.DTOs;
public class LoginDto
{
    [Required]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    public bool RememberMe { get; set; }
}