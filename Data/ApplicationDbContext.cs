using FlashcardApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FlashcardApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Flashcard> Flashcards { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Flashcard konfigürasyonu
            modelBuilder.Entity<Flashcard>(entity =>
            {
                entity.Property(f => f.FrontSide).HasMaxLength(500).IsRequired();
                entity.Property(f => f.BackSide).HasMaxLength(500).IsRequired();
                entity.Property(f => f.Category).HasMaxLength(100);
                entity.Property(f => f.CreatedDate).HasDefaultValueSql("GETDATE()");

                entity.HasOne(f => f.User)
                      .WithMany(u => u.Flashcards)
                      .HasForeignKey(f => f.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(f => f.CategoryObj)
                      .WithMany(c => c.Flashcards)
                      .HasForeignKey(f => f.CategoryId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            // Category konfigürasyonu
            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(c => c.Name).HasMaxLength(100).IsRequired();
                entity.Property(c => c.Description).HasMaxLength(500);
                entity.HasIndex(c => c.Name).IsUnique();
            });

            // ApplicationUser konfigürasyonu
            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.Property(u => u.FirstName).HasMaxLength(100);
                entity.Property(u => u.LastName).HasMaxLength(100);
                entity.Property(u => u.CreatedAt).HasDefaultValueSql("GETDATE()");
            });
        }
    }
}