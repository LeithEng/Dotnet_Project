using api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Linq.Expressions;
using api.Models;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Options;

namespace api.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }


        //public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Reaction> Reactions { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<UserEvent> UserEvents { get; set; }
        public DbSet<Hobby> Hobbies { get; set; }
        public DbSet<FavoriteHobby> FavoriteHobbies { get; set; }
        public DbSet<TokenBlacklist> TokenBlacklist { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var deletedAtProperty = entityType.FindProperty("DeletedAt");
                if (deletedAtProperty != null && deletedAtProperty.ClrType == typeof(DateTime?))
                {
                    var parameter = Expression.Parameter(entityType.ClrType, "e");
                    var property = Expression.Property(parameter, "DeletedAt");
                    var nullConstant = Expression.Constant(null, typeof(DateTime?));
                    var predicate = Expression.Lambda(Expression.Equal(property, nullConstant), parameter);

                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(predicate);
                }
            }

            base.OnModelCreating(modelBuilder);

            // Hobby Level Constraint
            modelBuilder.Entity<Hobby>()
                .ToTable(t => t.HasCheckConstraint("CK_Hobby_Level", "[Level] BETWEEN 1 AND 3"));

            modelBuilder.Entity<Hobby>()
                .Property(h => h.Level)
                .IsRequired()
                .HasDefaultValue(1);

            // FavoriteHobby relations
            modelBuilder.Entity<FavoriteHobby>()
                .HasKey(ue => new { ue.UserId, ue.HobbyId });

            modelBuilder.Entity<FavoriteHobby>()
                .HasOne(ue => ue.User)
                .WithMany(u => u.FavoriteHobbies)
                .HasForeignKey(ue => ue.UserId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<FavoriteHobby>()
                .HasOne(ue => ue.Hobby)
                .WithMany(e => e.FavoriteHobbies)
                .HasForeignKey(ue => ue.HobbyId);

            // Event-Hobby relation (One-to-many)
            modelBuilder.Entity<Event>()
                .HasOne(e => e.Hobby)
                .WithMany(h => h.Events)
                .HasForeignKey(e => e.HobbyId)
                .OnDelete(DeleteBehavior.Cascade);

            // Define the one-to-many relationship between Post and Hobby
            modelBuilder.Entity<Post>()
                .HasOne(p => p.Hobby)
                .WithMany(h => h.Posts)
                .HasForeignKey(p => p.HobbyId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Event>()
                .HasOne(e => e.User)
                .WithMany(u => u.CreatedEvents)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // PostComment relation
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Reactions relation
            modelBuilder.Entity<Reaction>()
                .HasOne(r => r.Post)
                .WithMany(p => p.Reactions)
                .HasForeignKey(r => r.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Reaction>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reactions)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // UserEvent Relationship
            modelBuilder.Entity<UserEvent>()
                .HasKey(ue => new { ue.UserId, ue.EventId });

            modelBuilder.Entity<UserEvent>()
                .HasOne(ue => ue.User)
                .WithMany(u => u.UserEvents)
                .HasForeignKey(ue => ue.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserEvent>()
                .HasOne(ue => ue.Event)
                .WithMany(e => e.UserEvents)
                .HasForeignKey(ue => ue.EventId);

            // Friend relationship
            modelBuilder.Entity<User>()
                .HasMany(u => u.Friends)
                .WithMany()
                .UsingEntity<Dictionary<string, object>>(
                    "UserFriendship",
                    j => j.HasOne<User>().WithMany().HasForeignKey("FriendId").OnDelete(DeleteBehavior.Restrict),
                    j => j.HasOne<User>().WithMany().HasForeignKey("UserId").OnDelete(DeleteBehavior.Restrict)
                );

            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Name = "User", NormalizedName = "USER" }
            );


            modelBuilder.Entity<Hobby>().HasData(
                            new Hobby
                            {
                                Id = "1",
                                IconPicture = "icon1.png",
                                Description = "Photography hobby",
                                Name = "Photography",
                                Level = 1
                            },
                            new Hobby
                            {
                                Id = "2",
                                IconPicture = "icon2.png",
                                Description = "Music hobby",
                                Name = "Music",
                                Level = 2
                            }
                        );

            // Seed Users
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = "user1",
                    UserName = "user1@example.com",
                    FirstName = "John",
                    LastName = "Doe",
                    IsPremium = true,
                    CreatedAt = DateTime.Now
                },
                new User
                {
                    Id = "user2",
                    UserName = "user2@example.com",
                    FirstName = "Jane",
                    LastName = "Smith",
                    IsPremium = false,
                    CreatedAt = DateTime.Now
                }
            );

            modelBuilder.Entity<FavoriteHobby>().HasData(
                new FavoriteHobby { HobbyId = "1", UserId = "user1" },
                new FavoriteHobby { HobbyId = "2", UserId = "user2" }
            );

            // Seed Events 
            modelBuilder.Entity<Event>().HasData(
                new Event
                {
                    Id = "event1",
                    Name = "Photography Workshop",
                    Description = "A workshop for photography enthusiasts.",
                    StartDate = DateTime.Now.AddDays(1),
                    EndDate = DateTime.Now.AddDays(2),
                    HobbyId = "1",
                    UserId = "user1"
                }
            );

            // Seed UserEvents
            modelBuilder.Entity<UserEvent>().HasData(
                new UserEvent { UserId = "user1", EventId = "event1", Rate = 5 }
            );

            // Seed Posts
            modelBuilder.Entity<Post>().HasData(
                new Post
                {
                    Id = "post1",
                    Content = "This is my first photography post!",
                    ImageUrl = "post1.jpg",
                    UserId = "user1",
                    HobbyId = "1"
                },
                new Post
                {
                    Id = "post2",
                    Content = "Check out my latest music creation!",
                    ImageUrl = "post2.jpg",
                    UserId = "user2",
                    HobbyId = "2"
                }
            );

            // Seed Comments
            modelBuilder.Entity<Comment>().HasData(
                new Comment
                {
                    Id = "comment1",
                    Content = "Amazing work! I love photography too.",
                    UserId = "user2",
                    PostId = "post1"
                },
                new Comment
                {
                    Id = "comment2",
                    Content = "Great music! Keep it up.",
                    UserId = "user1",
                    PostId = "post2"
                }
            );

            // Seed Reactions
            modelBuilder.Entity<Reaction>().HasData(
                new Reaction
                {
                    Id = "reaction1",
                    Type = "Like",
                    UserId = "user2",
                    PostId = "post1"
                },
                new Reaction
                {
                    Id = "reaction2",
                    Type = "Love",
                    UserId = "user1",
                    PostId = "post2"
                }
                );

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
            optionsBuilder.EnableSensitiveDataLogging();

        }
    }

}
