using Microsoft.Extensions.Hosting;
using NuGet.Protocol.Core.Types;
using api.Models;
using System.Drawing;
using Microsoft.AspNetCore.Identity;

namespace api.Models
{
    public class User : IdentityUser
    {
        public string? Avatar { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        //manest7akouch isVerified fama emailConfirmed min aand identity
        //public bool IsVerified { get; set; }=false;
        public bool IsPremium { get; set; } = false;
        public DateTime CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public ICollection<Post> Posts { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Reaction> Reactions { get; set; }
        public ICollection<UserEvent> UserEvents { get; set; }
        public ICollection<FavoriteHobby> FavoriteHobbies { get; set; }
        public ICollection<User> Friends { get; set; }

        public ICollection<Event> CreatedEvents { get; set; } 

    }



}
