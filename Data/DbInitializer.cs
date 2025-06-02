using FlashcardApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FlashcardApp.Data
{
    // Veritabanı başlangıç verilerini oluşturmak için static sınıf
    public static class DbInitializer
    {
        // Veritabanını başlatma ve seed verileri ekleme metodu
        public static void Initialize(ApplicationDbContext context)
        {
            // Veritabanını migrate et (gerekirse oluşturur)
            context.Database.Migrate();

            // 5. ADIM: Tüm mevcut verileri sil (sadece development ortamında)
            if (context.Database.IsSqlServer() && context.Database.CanConnect())
            {
                context.Flashcards.RemoveRange(context.Flashcards);
                context.SaveChanges();
            }

            // Eğer veritabanında hiç flashcard yoksa seed verileri ekle
            if (!context.Flashcards.Any())
            {
                var flashcards = new Flashcard[]
                {
                    new Flashcard
                    {
                        FrontSide = "ASP.NET Core nedir?",
                        BackSide = "Microsoft tarafından geliştirilen, açık kaynaklı ve platformlar arası bir web framework'üdür.",
                        Category = "Teknoloji",
                        CreatedDate = DateTime.Now.AddDays(-10),
                        LastReviewedDate = DateTime.Now.AddDays(-5)
                    },
                    new Flashcard
                    {
                        FrontSide = "Entity Framework Core nedir?",
                        BackSide = "Microsoft'un .NET uygulamaları için geliştirdiği bir ORM (Object-Relational Mapping) aracıdır.",
                        Category = "Teknoloji",
                        CreatedDate = DateTime.Now.AddDays(-8),
                        LastReviewedDate = DateTime.Now.AddDays(-3)
                    },
                    new Flashcard
                    {
                        FrontSide = "SOLID prensipleri nelerdir?",
                        BackSide = "1. Single Responsibility\n2. Open-Closed\n3. Liskov Substitution\n4. Interface Segregation\n5. Dependency Inversion",
                        Category = "Programlama",
                        CreatedDate = DateTime.Now.AddDays(-15),
                        LastReviewedDate = DateTime.Now.AddDays(-7)
                    },
                    new Flashcard
                    {
                        FrontSide = "İstanbul'un fethi ne zaman oldu?",
                        BackSide = "29 Mayıs 1453",
                        Category = "Tarih",
                        CreatedDate = DateTime.Now.AddDays(-20),
                        LastReviewedDate = DateTime.Now.AddDays(-2)
                    },
                    new Flashcard
                    {
                        FrontSide = "Newton'un hareket yasaları kaç tanedir?",
                        BackSide = "3 temel hareket yasası vardır.",
                        Category = "Fizik",
                        CreatedDate = DateTime.Now.AddDays(-12)
                    }
                };

                context.Flashcards.AddRange(flashcards);
                context.SaveChanges();
            }
        }
    }
}