using api.Models;
using System.ComponentModel.DataAnnotations;

namespace api.DTOs
{
    public class CreateAdmin
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
        public string Password { get; set; }
       
        [Required(ErrorMessage = "Role is required !")]
        public string role { get; set; }
        

    }
}
