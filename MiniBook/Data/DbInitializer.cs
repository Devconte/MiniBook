using MiniBook.Models;

namespace MiniBook.Data
{
    public static class DbInitializer
    {
        public static void Seed(AppDbContext context)
        {
            // Crée la base si elle n'existe pas encore
            context.Database.EnsureCreated();

            // Vérifie si un utilisateur existe déjà
            if (!context.Users.Any())
            {
                var demoUser = new User
                {
                    UserName = "demo",
                    Email = "demo@example.com",
                    PasswordHash = new byte[64], // tableau vide de 64 octets
                    CreatedAt = DateTime.UtcNow
                };

                context.Users.Add(demoUser);
                context.SaveChanges();
            }
        }
    }
}