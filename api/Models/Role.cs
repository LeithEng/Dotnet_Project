namespace api.Models
{
    public class Role : BaseSchema
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public ICollection<Permission> Permissions { get; set; }
    }
}
