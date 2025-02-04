using NuGet.Protocol.Core.Types;
using Newtonsoft.Json;

namespace api.Models
{
    public class Event : BaseSchema
    {
        public String Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string HobbyId { get; set; }
        public Hobby Hobby { get; set; }

        // Foreign Key for User (creator of the event)
        public string UserId { get; set; }
        public User User { get; set; }

        [JsonIgnore]
        public ICollection<UserEvent> UserEvents { get; set; }= new List<UserEvent>();
    }
}
