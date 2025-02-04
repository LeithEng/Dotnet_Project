using Newtonsoft.Json;

namespace api.Models
{
    public class Post : BaseSchema
    {
        public string Id { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }

        [JsonIgnore]
        public ICollection<Comment> Comments { get; set; }
        [JsonIgnore]
        public ICollection<Reaction> Reactions { get; set; }

        public string HobbyId { get; set; }  
        public Hobby Hobby { get; set; }     

    }

}
