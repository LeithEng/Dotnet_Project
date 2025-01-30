using System.ComponentModel.DataAnnotations;

namespace api.DTOs;
public class RegisterDto
{
    [Required(ErrorMessage = "FisrtName is required !")]
    public string FirstName { get; set; }
    [Required(ErrorMessage = "LastName is required !")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "Email is required !")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
    [Required(ErrorMessage = "Password is required !")]
    [DataType(DataType.Password)]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters long!")]
    public string Password { get; set; }
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Passwords do not match!")]
    public string ConfirmPassword { get; set; }
    public bool isAdmin { get; internal set; } = false;
}