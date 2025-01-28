namespace api.Models
{
    public abstract class BaseSchema
    {
        public DateTime CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

}
