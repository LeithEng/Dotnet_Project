namespace api.Models
{
    public class FavoriteHobby
    {
        public string UserId { get; set; }
        public User User { get; set; }
        public string HobbyId { get; set; }
        public Hobby Hobby { get; set; }
    }
}
