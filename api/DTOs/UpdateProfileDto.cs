using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
namespace api.DTOs;
public class UpdateProfileDto
{
    [Required(ErrorMessage = "first name is required!")]
    public string FirstName { get; set; }
    [Required(ErrorMessage = "last name is required!")]
    public string LastName { get; set; }
    [Required(ErrorMessage = "Email is required!")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
    [Required(ErrorMessage = "Username required sahbi ")]
    public string UserName { get; set; }
    [Required(ErrorMessage = "chnowa utilisateur ma3andouch password aya rigile ")]
    public string Password { get; set; }

    public byte[] Avatar { get; set; }

}