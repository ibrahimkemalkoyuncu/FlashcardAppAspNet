using FlashcardApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlashcardApp.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var services = scope.ServiceProvider;

            var context = services.GetRequiredService<ApplicationDbContext>();
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            await context.Database.MigrateAsync();

            await SeedRoles(roleManager);
            await SeedAdminUser(userManager, context);
            await SeedRegularUser(userManager, context);
        }

        private static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            string[] roles = { "Admin", "User" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        private static async Task SeedAdminUser(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            var admin = new ApplicationUser
            {
                UserName = "admin@flashcardapp.com",
                Email = "admin@flashcardapp.com",
                FirstName = "Admin",
                LastName = "User",
                EmailConfirmed = true,
                CreatedAt = DateTime.UtcNow
            };

            if (await userManager.FindByEmailAsync(admin.Email) == null)
            {
                await userManager.CreateAsync(admin, "Admin123!");
                await userManager.AddToRoleAsync(admin, "Admin");

                // Admin için örnek flashcard'lar
                var flashcards = new List<Flashcard>
                {
                    new Flashcard
                    {
                        FrontSide = "SOLID Prensipleri",
                        BackSide = "Single Responsibility, Open-Closed, Liskov Substitution, Interface Segregation, Dependency Inversion",
                        Category = "Programlama",
                        CreatedDate = DateTime.Now,
                        UserId = admin.Id
                    }
                };

                await context.Flashcards.AddRangeAsync(flashcards);
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedRegularUser(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            var user = new ApplicationUser
            {
                UserName = "user@flashcardapp.com",
                Email = "user@flashcardapp.com",
                FirstName = "Regular",
                LastName = "User",
                EmailConfirmed = true,
                CreatedAt = DateTime.UtcNow
            };

            if (await userManager.FindByEmailAsync(user.Email) == null)
            {
                await userManager.CreateAsync(user, "User123!");
                await userManager.AddToRoleAsync(user, "User");
            }
        }
    }
}