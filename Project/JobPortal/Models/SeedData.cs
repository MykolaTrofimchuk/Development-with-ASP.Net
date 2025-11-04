using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

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
                    PasswordHash = "12345",
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

            if (!context.Jobs.Any())
            {
                var jobs = new[]
                {
                    new Job
                    {
                        Title = "Frontend Developer",
                        Company = "TechNova",
                        Description = "Розробка інтерфейсів на React + TypeScript.",
                        DatePosted = DateTime.Now.AddDays(-5)
                    },
                    new Job
                    {
                        Title = "Backend Developer",
                        Company = "DataCore",
                        Description = "Розробка REST API на .NET Core, робота з SQL.",
                        DatePosted = DateTime.Now.AddDays(-3)
                    },
                    new Job
                    {
                        Title = "Project Manager",
                        Company = "SoftVision",
                        Description = "Координація команди, планування спринтів, робота з клієнтами.",
                        DatePosted = DateTime.Now.AddDays(-1)
                    }
                };

                context.Jobs.AddRange(jobs);
                context.SaveChanges();
            }

            if (!context.Applications.Any())
            {
                var job1 = context.Jobs.FirstOrDefault(j => j.Title == "Frontend Developer");
                var job2 = context.Jobs.FirstOrDefault(j => j.Title == "Backend Developer");

                var user2 = context.Users.FirstOrDefault(u => u.Email == "maria.savchuk@example.com");
                var user3 = context.Users.FirstOrDefault(u => u.Email == "oleh.koval@example.com");

                if (job1 != null && job2 != null && user2 != null && user3 != null)
                {
                    var applications = new[]
                    {
                        new Application
                        {
                            JobId = job1.Id,
                            // UserId = user2.Id,
                            FullName = "Іван Іваненко",
                            Email = "ivann@example.com",
                            ResumeText = "Маю досвід роботи з React та Vue, зацікавлена у вашій вакансії.",
                            DateApplied = DateTime.Now.AddDays(-2)
                        },
                        new Application
                        {
                            JobId = job2.Id,
                            // UserId = user3.Id,
                            FullName = "Петро Петренко",
                            Email = "pedro.petrenko@example.com",
                            ResumeText = "Працював із .NET понад 3 роки, маю досвід із EF Core та SQL Server.",
                            DateApplied = DateTime.Now.AddDays(-1)
                        }
                    };

                    context.Applications.AddRange(applications);
                    context.SaveChanges();
                }
            }
        }
    }
}
