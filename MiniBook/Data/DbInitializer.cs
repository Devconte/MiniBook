using MiniBook.Models;
using BCrypt.Net;

namespace MiniBook.Data
{
    public static class DbInitializer
    {
        public static void Seed(AppDbContext context)
        {
            // Crée la base si elle n'existe pas encore
            context.Database.EnsureCreated();

            // Vérifie si des utilisateurs existent déjà
            if (!context.Users.Any())
            {
                // --- Utilisateur standard ---
                var demoUser = new User
                {
                    UserName = "demo",
                    Email = "demo@example.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("demo123"),
                    Role = "User",
                    CreatedAt = DateTime.UtcNow
                };

                // --- Administrateur ---
                var adminUser = new User
                {
                    UserName = "admin",
                    Email = "admin@minibook.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                    Role = "Admin",
                    CreatedAt = DateTime.UtcNow
                };

                context.Users.AddRange(demoUser, adminUser);
                context.SaveChanges();
            }
        }
    }
}