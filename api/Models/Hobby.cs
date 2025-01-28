
namespace api.Models
{
    public class Hobby : BaseSchema
    {
        public string Id { get; set; }
        public string IconPicture { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public int Level { get; set; } 
        public string? ParentHobbyId { get; set; }
        public Hobby? ParentHobby { get; set; }
        public ICollection<Hobby>? SubHobbies { get; set; }
    }
}
