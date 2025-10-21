using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace JobPortal.Models
{
    public static class SeedData
    {
        public static void EnsurePopulated(IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<PortalDbContext>();

            // Якщо є незастосовані міграції — застосувати
            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }

            if (!context.Users.Any())
            {
                var user1 = new User
                {
                    FullName = "Іван Петренко",
                    Email = "ivan.petrenko@example.com",
                    PasswordHash = "12345", // тимчасово, для демо
                    Role = "Employer",
                    CreatedAt = DateTime.UtcNow
                };

                var user2 = new User
                {
                    FullName = "Марія Савчук",
                    Email = "maria.savchuk@example.com",
                    PasswordHash = "12345",
                    Role = "Candidate",
                    CreatedAt = DateTime.UtcNow
                };

                var user3 = new User
                {
                    FullName = "Олег Коваль",
                    Email = "oleh.koval@example.com",
                    PasswordHash = "12345",
                    Role = "Candidate",
                    CreatedAt = DateTime.UtcNow
                };

                var user4 = new User
                {
                    FullName = "Катерина Катренко",
                    Email = "katya@example.com",
                    PasswordHash = "123123",
                    Role = "Employer",
                    CreatedAt = DateTime.UtcNow
                };

                context.Users.AddRange(user1, user2, user3, user4);
                context.SaveChanges();
            }
        }
    }
}
