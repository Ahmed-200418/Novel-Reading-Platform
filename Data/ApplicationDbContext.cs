using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NovelPlatform.Models;

namespace NovelPlatform.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Novel> Novels { get; set; } = null!;
        public DbSet<Chapter> Chapters { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<Payment> Payments { get; set; } = null!;
        public DbSet<Purchase> Purchases { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure Purchase uniqueness per user and chapter
            builder.Entity<Purchase>()
                .HasIndex(p => new { p.UserId, p.ChapterId })
                .IsUnique();

            // Configure relationships
            builder.Entity<Chapter>()
                .HasOne(c => c.Novel)
                .WithMany(n => n.Chapters)
                .HasForeignKey(c => c.NovelId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Order>()
                .HasOne(o => o.Chapter)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.ChapterId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Purchase>()
                .HasOne(p => p.Chapter)
                .WithMany(c => c.Purchases)
                .HasForeignKey(p => p.ChapterId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Purchase>()
                .HasOne(p => p.User)
                .WithMany(u => u.Purchases)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Payment>()
                .HasOne(p => p.Order)
                .WithOne(o => o.Payment)
                .HasForeignKey<Payment>(p => p.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
