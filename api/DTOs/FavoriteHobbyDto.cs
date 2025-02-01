using System.ComponentModel.DataAnnotations;
namespace api.DTOs
{
    public class FavoriteHobbyDto
    {
        [Required(ErrorMessage = "userId required")]
        public string UserId { get; set; }

        [Required (ErrorMessage ="HobbyId required")]
        public string HobbyId { get; set; }    
      
    }
}
