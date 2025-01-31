using System;

namespace api.DTOs
{
    public class PostDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }

        public string Content { get; set; }
        public string UserId { get; set; } 
        public string HobbyId { get; set; }  
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
