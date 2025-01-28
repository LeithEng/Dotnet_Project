namespace api.Models
{
    public class Permission : BaseSchema
    {
        public string Id { get; set; }
        public string Action { get; set; }
        public string RoleId { get; set; }
        public Role Role { get; set; }
    }
}
