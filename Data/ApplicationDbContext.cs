using Microsoft.EntityFrameworkCore;
using FlashcardApp.Models;

namespace FlashcardApp.Data
{
    // Entity Framework Core DbContext sınıfımız
    public class ApplicationDbContext : DbContext
    {
        // Constructor: DbContextOptions alır ve base sınıfa iletir
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Flashcards tablosunu temsil eden DbSet
        public DbSet<Flashcard> Flashcards { get; set; }

        // Model oluşturulurken ek konfigürasyonlar yapabilmek için
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Flashcard entity için ek konfigürasyonlar
            modelBuilder.Entity<Flashcard>(entity =>
            {
                // FrontSide için maksimum uzunluk
                entity.Property(e => e.FrontSide).HasMaxLength(500);
                
                // BackSide için maksimum uzunluk
                entity.Property(e => e.BackSide).HasMaxLength(500);
                
                // Category için maksimum uzunluk
                entity.Property(e => e.Category).HasMaxLength(100);
            });
        }
    }
}