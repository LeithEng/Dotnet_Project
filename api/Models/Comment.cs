﻿namespace api.Models
{
    public class Comment : BaseSchema
    {
        public String Id { get; set; }
        public string Content { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public string PostId { get; set; }
        public Post Post { get; set; }
    }

}
