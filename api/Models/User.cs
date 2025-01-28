using Microsoft.Extensions.Hosting;
using NuGet.Protocol.Core.Types;
using api.Models;
using System.Drawing;
using Microsoft.AspNetCore.Identity;

namespace api.Models
{
    public class User : IdentityUser
    {
        //public string Id { get; set; }
        public string Avatar { get; set; }
        //public string Email { get; set; }
        //public string FirstName { get; set; }
        //public string LastName { get; set; }
        public bool IsVerified { get; set; }
        public bool IsPremium { get; set; }
        //public string Password { get; set; }
        //public string Username { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Relationships
        public ICollection<Post> Posts { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Reaction> Reactions { get; set; }
        public ICollection<UserEvent> UserEvents { get; set; }
        public ICollection<FavoriteHobby> FavoriteHobbies { get; set; }

        // Self-referencing many-to-many relationship for friendships
        public ICollection<User> Friends { get; set; }
    }



}
