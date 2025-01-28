namespace api.Models
{
    public class FavoriteHobby : Hobby
    {
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
