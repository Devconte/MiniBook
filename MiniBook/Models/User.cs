
using System.ComponentModel.DataAnnotations;

namespace MiniBook.Models;

public class User
{
    public int Id { get; set; }

    [Required, StringLength(50)]
    public string UserName { get; set; } = null!;

    [Required, StringLength(255), EmailAddress]
    public string Email { get; set; } = null!;

    // Hash stocké en base (ex: PBKDF2/BCrypt -> byte[])
    [Required]
    public byte[] PasswordHash { get; set; } = Array.Empty<byte>();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigations
    public UserProfile? Profile { get; set; }
    public ICollection<Post> Posts { get; set; } = new List<Post>();
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
}