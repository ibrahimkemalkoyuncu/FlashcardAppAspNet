using FlashcardApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlashcardApp.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider, bool isDevelopment = true)
        {
            using var scope = serviceProvider.CreateScope();
            var services = scope.ServiceProvider;

            var context = services.GetRequiredService<ApplicationDbContext>();
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            await context.Database.MigrateAsync();

            if (isDevelopment)
            {
                await SeedRolesAsync(roleManager);
                await SeedAdminUserAsync(userManager, context, roleManager);
                await SeedRegularUsersAsync(userManager, context);
                await SeedCategoriesAsync(context);
                await SeedFlashcardsAsync(context);
            }
        }

        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            var roles = new List<string> { "Admin", "PremiumUser", "User" };

            foreach (var roleName in roles)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }

        private static async Task SeedAdminUserAsync(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context,
            RoleManager<IdentityRole> roleManager)
        {
            const string adminEmail = "admin@flashcardapp.com";
            const string adminPassword = "Admin123!";
            const string adminRole = "Admin";
            const string premiumRole = "PremiumUser";

            // Create roles if they don't exist
            if (!await roleManager.RoleExistsAsync(adminRole))
            {
                await roleManager.CreateAsync(new IdentityRole(adminRole));
            }
            if (!await roleManager.RoleExistsAsync(premiumRole))
            {
                await roleManager.CreateAsync(new IdentityRole(premiumRole));
            }

            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = "Admin",
                    LastName = "User",
                    EmailConfirmed = true,
                    CreatedDate = DateTime.UtcNow,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    ProfilePicture = "/images/HFZH7561.JPG"
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (!result.Succeeded)
                {
                    throw new Exception($"Admin user creation failed: {string.Join(", ", result.Errors)}");
                }

                await userManager.AddToRolesAsync(adminUser, new[] { adminRole, premiumRole });
            }
            else
            {
                // Ensure existing admin has both roles
                if (!await userManager.IsInRoleAsync(adminUser, adminRole))
                {
                    await userManager.AddToRoleAsync(adminUser, adminRole);
                }
                if (!await userManager.IsInRoleAsync(adminUser, premiumRole))
                {
                    await userManager.AddToRoleAsync(adminUser, premiumRole);
                }
            }
        }

        private static async Task SeedRegularUsersAsync(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            var users = new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    UserName = "user1@flashcardapp.com",
                    Email = "user1@flashcardapp.com",
                    FirstName = "Ahmet",
                    LastName = "Yılmaz",
                    EmailConfirmed = true,
                    CreatedDate = DateTime.UtcNow.AddDays(-10),
                    SecurityStamp = Guid.NewGuid().ToString()
                },
                new ApplicationUser
                {
                    UserName = "user2@flashcardapp.com",
                    Email = "user2@flashcardapp.com",
                    FirstName = "Ayşe",
                    LastName = "Kaya",
                    EmailConfirmed = true,
                    CreatedDate = DateTime.UtcNow.AddDays(-5),
                    SecurityStamp = Guid.NewGuid().ToString()
                },
                new ApplicationUser
                {
                    UserName = "user3@flashcardapp.com",
                    Email = "user3@flashcardapp.com",
                    FirstName = "Mehmet",
                    LastName = "Demir",
                    EmailConfirmed = true,
                    CreatedDate = DateTime.UtcNow.AddDays(-3),
                    SecurityStamp = Guid.NewGuid().ToString()
                }
            };

            foreach (var user in users)
            {
                if (await userManager.FindByEmailAsync(user.Email) == null)
                {
                    await userManager.CreateAsync(user, "User123!");
                    await userManager.AddToRoleAsync(user, "User");
                }
            }
        }

        private static async Task SeedCategoriesAsync(ApplicationDbContext context)
        {
            if (await context.Categories.AnyAsync()) return;

            // Örneğin ilk kullanıcıyı alalım
            var user = await context.Users.FirstOrDefaultAsync();
            if (user == null) return; // Kullanıcı yoksa ekleme yapma

            var categories = new List<Category>
    {
        new Category { Name = "Programlama", Description = "Yazılım geliştirme ile ilgili terimler", UserId = user.Id },
        new Category { Name = "Matematik", Description = "Matematiksel kavramlar ve formüller", UserId = user.Id },
        new Category { Name = "Tarih", Description = "Tarihi olaylar ve kişiler", UserId = user.Id },
        new Category { Name = "Bilim", Description = "Bilimsel terimler ve keşifler", UserId = user.Id },
        new Category { Name = "Yabancı Dil", Description = "Yabancı dil öğrenme kartları", UserId = user.Id },
        new Category { Name = "Coğrafya", Description = "Ülkeler, şehirler ve coğrafi özellikler", UserId = user.Id },
        new Category { Name = "Sanat", Description = "Sanat tarihi ve terimleri", UserId = user.Id },
        new Category { Name = "Spor", Description = "Spor dalları ve terimleri", UserId = user.Id }
    };

            await context.Categories.AddRangeAsync(categories);
            await context.SaveChangesAsync();
        }

        private static async Task SeedFlashcardsAsync(ApplicationDbContext context)
        {
            if (await context.Flashcards.AnyAsync()) return;

            var users = await context.Users.ToListAsync();
            var categories = await context.Categories.ToListAsync();
            var random = new Random();

            var flashcards = new List<Flashcard>();

            foreach (var user in users)
            {
                // Create 10-15 flashcards per user
                var flashcardCount = random.Next(10, 16);

                for (int i = 0; i < flashcardCount; i++)
                {
                    var category = categories[random.Next(categories.Count)];
                    var createdDate = DateTime.UtcNow.AddDays(-random.Next(1, 30));
                    var lastReviewedDate = random.Next(3) == 0 ? null : (DateTime?)createdDate.AddDays(random.Next(1, 15));

                    flashcards.Add(new Flashcard
                    {
                        FrontSide = GenerateFrontSide(category.Name),
                        BackSide = GenerateBackSide(category.Name),
                        CategoryId = category.Id,
                        UserId = user.Id,
                        CreatedDate = createdDate,
                        LastReviewedDate = lastReviewedDate,
                        DifficultyLevel = random.Next(1, 4)
                    });
                }
            }

            await context.Flashcards.AddRangeAsync(flashcards);
            await context.SaveChangesAsync();
        }

        // Flashcard content generators
        private static readonly string[] programmingFrontSides = {
            "SOLID prensipleri nelerdir?",
            "Entity Framework Core nedir?",
            "ASP.NET Core middleware'i nedir?",
            "Dependency Injection nedir?",
            "Repository pattern nedir?"
        };

        private static readonly string[] programmingBackSides = {
            "Single Responsibility, Open-Closed, Liskov Substitution, Interface Segregation, Dependency Inversion",
            "Microsoft'un .NET uygulamaları için geliştirdiği bir ORM (Object-Relational Mapping) aracıdır",
            "HTTP istek pipeline'ını oluşturan ve istekleri işleyen bileşenlerdir",
            "Bağımlılıkların dışarıdan enjekte edilerek sıkı bağlılığı azaltan bir tasarım desenidir",
            "Veri erişim katmanını soyutlayarak business logic'ten ayıran bir tasarım desenidir"
        };

        private static readonly string[] mathFrontSides = {
            "Pisagor teoremi nedir?",
            "İntegral neyi hesaplar?",
            "Asal sayı nedir?",
            "Trigonometri nedir?",
            "Türev neyi ifade eder?"
        };

        private static readonly string[] mathBackSides = {
            "Bir dik üçgende hipotenüsün karesi diğer iki kenarın kareleri toplamına eşittir (a² + b² = c²)",
            "Bir fonksiyonun altındaki alanı hesaplar",
            "1 ve kendisinden başka pozitif böleni olmayan sayılardır",
            "Üçgenlerin açı ve kenar ilişkilerini inceleyen matematik dalıdır",
            "Bir fonksiyonun değişim oranını ifade eder"
        };

        private static readonly string[] historyFrontSides = {
            "İstanbul'un fethi hangi yılda oldu?",
            "Kurtuluş Savaşı ne zaman başladı?",
            "Fransız İhtilali'nin önemi nedir?",
            "Sanayi Devrimi ne zaman başladı?",
            "Rönesans dönemi nedir?"
        };

        private static readonly string[] historyBackSides = {
            "1453 yılında Fatih Sultan Mehmet tarafından fethedildi",
            "1919 yılında Mustafa Kemal Atatürk önderliğinde başladı",
            "1789'da başlayan ve demokrasi, eşitlik gibi kavramları yaygınlaştıran devrim",
            "18. yüzyılda İngiltere'de başlayan üretim devrimi",
            "14-17. yüzyıllar arasında Avrupa'da yaşanan kültürel ve sanatsal yeniden doğuş dönemi"
        };

        private static readonly string[] defaultFrontSides = {
            "Örnek flashcard ön yüz",
            "Temel bilgi kartı",
            "Genel kültür sorusu"
        };

        private static readonly string[] defaultBackSides = {
            "Örnek flashcard arka yüz",
            "Temel bilgi cevabı",
            "Genel kültür cevabı"
        };

        private static string GenerateFrontSide(string categoryName)
        {
            var random = new Random();
            return categoryName switch
            {
                "Programlama" => programmingFrontSides[random.Next(programmingFrontSides.Length)],
                "Matematik" => mathFrontSides[random.Next(mathFrontSides.Length)],
                "Tarih" => historyFrontSides[random.Next(historyFrontSides.Length)],
                _ => defaultFrontSides[random.Next(defaultFrontSides.Length)]
            };
        }

        private static string GenerateBackSide(string categoryName)
        {
            var random = new Random();
            return categoryName switch
            {
                "Programlama" => programmingBackSides[random.Next(programmingBackSides.Length)],
                "Matematik" => mathBackSides[random.Next(mathBackSides.Length)],
                "Tarih" => historyBackSides[random.Next(historyBackSides.Length)],
                _ => defaultBackSides[random.Next(defaultBackSides.Length)]
            };
        }
    }
}