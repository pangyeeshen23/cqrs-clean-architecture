using Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure.Context
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
            
        }
        public DbSet<User> Users { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostTags> PostTags { get; set; }
        public DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>()
                .HasIndex(e => e.Username)
                .IsUnique();

            modelBuilder.Entity<PostTags>()
             .HasOne(pt => pt.Post)
             .WithMany(p => p.PostTags)
             .HasForeignKey(pt => pt.PostId)
             .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PostTags>()
             .HasOne(pt => pt.Tag)
             .WithMany(t => t.PostTags)
             .HasForeignKey(pt => pt.TagId)
             .OnDelete(DeleteBehavior.NoAction);
        }

        public override int SaveChanges()
        {
            var entries = ChangeTracker.Entries<AuditableEntity>();
            foreach(var entry in entries)
            {
                if (entry.State == EntityState.Modified) entry.Entity.UpdateAt = DateTime.UtcNow;
                if (entry.State == EntityState.Added) entry.Entity.CreatedAt = DateTime.UtcNow;
            }
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<AuditableEntity>();
            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Modified) entry.Entity.UpdateAt = DateTime.UtcNow;
                if (entry.State == EntityState.Added) entry.Entity.CreatedAt = DateTime.UtcNow;
            }
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

    }
}
