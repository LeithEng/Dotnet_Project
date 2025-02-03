using api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Linq.Expressions;
using api.Models;
//using 

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
        }


    }

}
