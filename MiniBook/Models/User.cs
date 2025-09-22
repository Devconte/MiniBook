
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
    
    // 👉 Navigations liées à Friendship
    public ICollection<Friendship> FriendshipsSent { get; set; } = new List<Friendship>();
    public ICollection<Friendship> FriendshipsReceived { get; set; } = new List<Friendship>();

    // 👉 Méthode AjouterAmi
    public Friendship AddFriend(User addressee)
    {
        if (addressee == null) throw new ArgumentNullException(nameof(addressee));
        if (addressee.Id == Id) throw new InvalidOperationException("You cannot add yourself as a friend.");

        var friendship = new Friendship
        {
            Requester = this,
            RequesterId = this.Id,
            Addressee = addressee,
            AddresseeId = addressee.Id,
            Status = FriendshipStatus.Pending,
            RequestedAt = DateTime.UtcNow
        };

        FriendshipsSent.Add(friendship);
        return friendship;
    }
}