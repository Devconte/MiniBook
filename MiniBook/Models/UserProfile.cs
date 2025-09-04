using System.ComponentModel.DataAnnotations;
namespace MiniBook.Models;


public class UserProfile
{
    // PK = FK vers User (relation 1–1)
    [Key]
    public int UserId { get; set; }

    [StringLength(500)]
    public string? Bio { get; set; }

    [StringLength(400), Url]
    public string? AvatarUrl { get; set; }

    // Navigation
    public User User { get; set; } = null!;
}