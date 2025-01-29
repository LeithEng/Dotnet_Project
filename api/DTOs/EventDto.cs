namespace api.DTOs
{
    public class EventDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string HobbyId { get; set; }
        public List<string> UserIds { get; set; }
        public string CreatedByUserId { get; set; } // Optional: if you want to track which user created the event

    }
}


