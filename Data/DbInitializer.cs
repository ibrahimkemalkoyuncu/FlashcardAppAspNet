using FlashcardApp.Models;
using FlashcardApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FlashcardApp.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            bool isDevelopment)
        {
            // Veritabanını otomatik migrate et
            await context.Database.MigrateAsync();

            // Development ortamında seed verileri ekle
            if (isDevelopment)
            {
                await SeedRolesAsync(roleManager);
                await SeedAdminUserAsync(userManager);
                await SeedFlashcardsAsync(context);
            }
        }

        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            string[] roleNames = { "Admin", "User" };

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }

        private static async Task SeedAdminUserAsync(UserManager<ApplicationUser> userManager)
        {
            var adminUser = new ApplicationUser
            {
                UserName = "admin@flashcardapp.com",
                Email = "admin@flashcardapp.com",
                EmailConfirmed = true
            };

            var user = await userManager.FindByEmailAsync(adminUser.Email);
            if (user == null)
            {
                var createResult = await userManager.CreateAsync(adminUser, "Admin123!");
                if (createResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }

        private static async Task SeedFlashcardsAsync(ApplicationDbContext context)
        {
            // Kategorileri önce oluştur
            if (!await context.Categories.AnyAsync())
            {
                var categories = new List<Category>
                {
                    new Category { Name = "Teknoloji" },
                    new Category { Name = "Programlama" },
                    new Category { Name = "Tarih" },
                    new Category { Name = "Fizik" }
                };

                await context.Categories.AddRangeAsync(categories);
                await context.SaveChangesAsync();
            }

            // Flashcard'ları ekle (kategorilerle ilişkilendirerek)
            if (!await context.Flashcards.AnyAsync())
            {
                var techCategory = await context.Categories.FirstAsync(c => c.Name == "Teknoloji");
                var programmingCategory = await context.Categories.FirstAsync(c => c.Name == "Programlama");
                var historyCategory = await context.Categories.FirstAsync(c => c.Name == "Tarih");
                var physicsCategory = await context.Categories.FirstAsync(c => c.Name == "Fizik");

                var flashcards = new List<Flashcard>
                {
                    new Flashcard
                    {
                        FrontSide = "ASP.NET Core nedir?",
                        BackSide = "Microsoft tarafından geliştirilen, açık kaynaklı ve platformlar arası bir web framework'üdür.",
                        CategoryId = techCategory.Id,
                        CreatedDate = DateTime.Now.AddDays(-10),
                        LastReviewedDate = DateTime.Now.AddDays(-5)
                    },
                    new Flashcard
                    {
                        FrontSide = "Entity Framework Core nedir?",
                        BackSide = "Microsoft'un .NET uygulamaları için geliştirdiği bir ORM (Object-Relational Mapping) aracıdır.",
                        CategoryId = techCategory.Id,
                        CreatedDate = DateTime.Now.AddDays(-8),
                        LastReviewedDate = DateTime.Now.AddDays(-3)
                    },
                    new Flashcard
                    {
                        FrontSide = "SOLID prensipleri nelerdir?",
                        BackSide = "1. Single Responsibility\n2. Open-Closed\n3. Liskov Substitution\n4. Interface Segregation\n5. Dependency Inversion",
                        CategoryId = programmingCategory.Id,
                        CreatedDate = DateTime.Now.AddDays(-15),
                        LastReviewedDate = DateTime.Now.AddDays(-7)
                    },
                    new Flashcard
                    {
                        FrontSide = "İstanbul'un fethi ne zaman oldu?",
                        BackSide = "29 Mayıs 1453",
                        CategoryId = historyCategory.Id,
                        CreatedDate = DateTime.Now.AddDays(-20),
                        LastReviewedDate = DateTime.Now.AddDays(-2)
                    },
                    new Flashcard
                    {
                        FrontSide = "Newton'un hareket yasaları kaç tanedir?",
                        BackSide = "3 temel hareket yasası vardır.",
                        CategoryId = physicsCategory.Id,
                        CreatedDate = DateTime.Now.AddDays(-12)
                    }
                };

                await context.Flashcards.AddRangeAsync(flashcards);
                await context.SaveChangesAsync();
            }
        }
    }
}