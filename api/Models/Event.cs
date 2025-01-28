using NuGet.Protocol.Core.Types;

namespace api.Models
{
    public class Event : BaseSchema
    {
        public String Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // Relationships
        public ICollection<UserEvent> UserEvents { get; set; }
    }
}
