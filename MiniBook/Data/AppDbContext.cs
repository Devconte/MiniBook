using Microsoft.EntityFrameworkCore;
using MiniBook.Models;

namespace MiniBook.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        // Tables
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<UserProfile> UserProfiles { get; set; } = null!;
        public DbSet<Post> Posts { get; set; } = null!;
        public DbSet<Comment> Comments { get; set; } = null!;
        public DbSet<Friendship> Friendships { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder b)
        {
            base.OnModelCreating(b);

            // ===== USER =====
            b.Entity<User>(e =>
            {
                e.ToTable("Users");
                e.HasKey(x => x.Id);

                e.Property(x => x.UserName)
                    .HasMaxLength(50)
                    .IsRequired();

                e.Property(x => x.Email)
                    .HasMaxLength(255)
                    .IsRequired();

                e.Property(x => x.PasswordHash)
                    .HasColumnType("varbinary(64)")
                    .IsRequired();

                e.Property(x => x.CreatedAt)
                    .HasColumnType("datetime2(0)")
                    .HasDefaultValueSql("SYSUTCDATETIME()");

                e.HasIndex(x => x.UserName).IsUnique();
                e.HasIndex(x => x.Email).IsUnique();

                e.HasOne(x => x.Profile)
                 .WithOne(p => p.User)
                 .HasForeignKey<UserProfile>(p => p.UserId)
                 .OnDelete(DeleteBehavior.Cascade); // supprimer le profil avec l'utilisateur
            });

            // ===== USER PROFILE (1-1) =====
            b.Entity<UserProfile>(e =>
            {
                e.ToTable("UserProfiles");
                e.HasKey(x => x.UserId); // PK = FK

                e.Property(x => x.Bio).HasMaxLength(500);
                e.Property(x => x.AvatarUrl).HasMaxLength(400);
            });

            // ===== POST (User 1-N) =====
            b.Entity<Post>(e =>
            {
                e.ToTable("Posts");
                e.HasKey(x => x.Id);

                e.Property(x => x.Title)
                    .HasMaxLength(120)
                    .IsRequired();

                e.Property(x => x.Content)
                    .HasColumnType("nvarchar(max)")
                    .IsRequired();

                e.Property(x => x.CreatedAt)
                    .HasColumnType("datetime2(0)")
                    .HasDefaultValueSql("SYSUTCDATETIME()");

                e.HasOne(x => x.User)
                 .WithMany(u => u.Posts)
                 .HasForeignKey(x => x.UserId)
                 .OnDelete(DeleteBehavior.Cascade); // supprimer les posts si l'user est supprimé

                e.HasIndex(x => new { x.UserId, x.CreatedAt });
            });

            // ===== COMMENT (Post 1-N, User 1-N) =====
            b.Entity<Comment>(e =>
            {
                e.ToTable("Comments");
                e.HasKey(x => x.Id);

                e.Property(x => x.Content)
                    .HasMaxLength(1000)
                    .IsRequired();

                e.Property(x => x.CreatedAt)
                    .HasColumnType("datetime2(0)")
                    .HasDefaultValueSql("SYSUTCDATETIME()");

                // Comment -> Post : CASCADE ok
                e.HasOne(x => x.Post)
                 .WithMany(p => p.Comments)
                 .HasForeignKey(x => x.PostId)
                 .OnDelete(DeleteBehavior.Cascade);

                // Comment -> User (Author) : RESTRICT pour éviter multiple cascade paths
                e.HasOne(x => x.Author)
                 .WithMany(u => u.Comments)
                 .HasForeignKey(x => x.AuthorId)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasIndex(x => new { x.PostId, x.CreatedAt });
            });

            // ===== FRIENDSHIP (User <-> User) =====
            b.Entity<Friendship>(e =>
            {
                e.ToTable("Friendships");
                e.HasKey(x => x.Id);

                e.Property(x => x.Status)
                    .HasColumnType("tinyint"); // map enum byte

                e.Property(x => x.RequestedAt)
                    .HasColumnType("datetime2(0)")
                    .HasDefaultValueSql("SYSUTCDATETIME()");

                // éviter cascades croisées : Restrict des deux côtés
                e.HasOne(x => x.Requester)
                 .WithMany()
                 .HasForeignKey(x => x.RequesterId)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(x => x.Addressee)
                 .WithMany()
                 .HasForeignKey(x => x.AddresseeId)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasIndex(x => new { x.RequesterId, x.AddresseeId }).IsUnique();
            });
        }
    }
}
