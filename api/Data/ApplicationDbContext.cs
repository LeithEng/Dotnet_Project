using api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

       
        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Reaction> Reactions { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<UserEvent> UserEvents { get; set; }
        public DbSet<Hobby> Hobbies { get; set; }
        public DbSet<FavoriteHobby> FavoriteHobbies { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Hobby Level Constraint
            modelBuilder.Entity<Hobby>()
                .ToTable(t => t.HasCheckConstraint("CK_Hobby_Level", "[Level] BETWEEN 1 AND 3"));

            modelBuilder.Entity<Hobby>()
                .Property(h => h.Level)
                .IsRequired()
                .HasDefaultValue(1);

            // UserFavoriteHobby relation
            modelBuilder.Entity<FavoriteHobby>()
                .HasOne(fh => fh.User)
                .WithMany(u => u.FavoriteHobbies)
                .HasForeignKey(fh => fh.UserId);

            // HobbySubHobbies relation
            modelBuilder.Entity<Hobby>()
                .HasOne(h => h.ParentHobby)
                .WithMany(h => h.SubHobbies)
                .HasForeignKey(h => h.ParentHobbyId)
                .OnDelete(DeleteBehavior.Restrict);

            // PostComment relation
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Reactions relation
            modelBuilder.Entity<Reaction>()
                .HasOne(r => r.Post)
                .WithMany(p => p.Reactions)
                .HasForeignKey(r => r.PostId)
                .OnDelete(DeleteBehavior.Restrict); // Avoid cascading deletes

            modelBuilder.Entity<Reaction>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reactions)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Avoid cascading deletes

            // UserEvent Relationship
            modelBuilder.Entity<UserEvent>()
                .HasKey(ue => new { ue.UserId, ue.EventId });

            modelBuilder.Entity<UserEvent>()
                .HasOne(ue => ue.User)
                .WithMany(u => u.UserEvents)
                .HasForeignKey(ue => ue.UserId);

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
        }


    }

}
