using System.ComponentModel.DataAnnotations;
namespace api.DTOs
{
    public class HobbyDto
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string? Description { get; set; }

        public byte[]? IconPicture { get; set; }

        [Required(ErrorMessage = "Level is required")]
        public int Level { get; set; }
        public string? ParentHobbyId { get; set; }
    }
}
