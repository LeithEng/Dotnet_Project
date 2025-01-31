using System.ComponentModel.DataAnnotations;
namespace api.DTOs
{
    public class ReactionDto
    {
        [Required (ErrorMessage = "Type is required")]
        public string Type { get; set; }

        [Required(ErrorMessage = "UserId is required")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "PostId is required")]
        public string PostId { get; set; }
    }
}
