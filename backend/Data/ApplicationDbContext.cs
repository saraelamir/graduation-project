using Microsoft.EntityFrameworkCore;
using GraduProjj.Models;

namespace GraduProjj.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Define DbSets for each table
        public DbSet<User> Users { get; set; }
        public DbSet<Input> Inputs { get; set; }
        public DbSet<Goal> Goals { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Users table
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.UserId);

                // Relationships for the User entity
                entity.HasMany(u => u.Goals)
                    .WithOne(g => g.User)
                    .HasForeignKey(g => g.UserID)
                    .OnDelete(DeleteBehavior.Restrict);

                //entity.HasMany(u => u.Plans)
                //    .WithOne(p => p.User)
                //    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(u => u.Input)
                    .WithOne(i => i.User)
                    .HasForeignKey(i => i.UserID)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(u => u.Feedbacks)
                    .WithOne(f => f.User)
                    .HasForeignKey(f => f.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure Inputs table
            modelBuilder.Entity<Input>(entity =>
            {
                entity.HasOne(i => i.User)
                      .WithMany(u => u.Input)
                      .HasForeignKey(i => i.UserID)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure Goals table
            modelBuilder.Entity<Goal>(entity =>
            {
                entity.HasOne(g => g.User)
                    .WithMany(u => u.Goals)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure Plans table
            modelBuilder.Entity<Plan>(entity =>
            { 

                entity.HasOne(p => p.Goal)
                    .WithMany(g => g.Plans)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure Feedbacks table
            modelBuilder.Entity<Feedback>(entity =>
            {
                entity.HasOne(f => f.User)
                    .WithMany(u => u.Feedbacks)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Indexes for performance optimization
            modelBuilder.Entity<Goal>()
                .HasIndex(g => g.Status);

            modelBuilder.Entity<Plan>()
                .HasIndex(p => p.Status);

            modelBuilder.Entity<Feedback>()
                .HasIndex(f => f.SubmissionDate);
        }
    }
}
