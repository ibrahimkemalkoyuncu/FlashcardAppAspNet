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
                // FrontSide ve BackSide için maksimum uzunluk
                entity.Property(f => f.FrontSide).HasMaxLength(500).IsRequired();
                entity.Property(f => f.BackSide).HasMaxLength(500).IsRequired();

                // Zorluk seviyesi için varsayılan değer
                entity.Property(f => f.DifficultyLevel).HasDefaultValue(3);

                // Kullanıcı ilişkisi - CASCADE silme
                entity.HasOne(f => f.User)
                      .WithMany(u => u.Flashcards)
                      .HasForeignKey(f => f.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Kategori ilişkisi - SET NULL silme (düzeltilmiş)
                entity.HasOne(f => f.Category)
                      .WithMany(c => c.Flashcards)
                      .HasForeignKey(f => f.CategoryId)
                      .OnDelete(DeleteBehavior.ClientSetNull); // SQL Server için düzeltme
            });

            // Category konfigürasyonu
            modelBuilder.Entity<Category>(entity =>
            {
                // Name ve Description için maksimum uzunluk
                entity.Property(c => c.Name).HasMaxLength(100).IsRequired();
                entity.Property(c => c.Description).HasMaxLength(500);

                // Kullanıcıya göre benzersiz kategori ismi
                entity.HasIndex(c => new { c.Name, c.UserId }).IsUnique();

                // Kullanıcı ilişkisi - CASCADE silme
                entity.HasOne(c => c.User)
                      .WithMany(u => u.Categories)
                      .HasForeignKey(c => c.UserId)
                       //.OnDelete(DeleteBehavior.Cascade);
                       .OnDelete(DeleteBehavior.Restrict); // Cascade yerine Restrict kullanın
            });

            // ApplicationUser konfigürasyonu
            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                // Temel bilgiler için maksimum uzunluk
                entity.Property(u => u.FirstName).HasMaxLength(100);
                entity.Property(u => u.LastName).HasMaxLength(100);
                entity.Property(u => u.ProfilePicture).HasMaxLength(255);

                // Oluşturma tarihi için varsayılan değer
                entity.Property(u => u.CreatedDate).HasDefaultValueSql("GETDATE()");
            });
        }
    }
}