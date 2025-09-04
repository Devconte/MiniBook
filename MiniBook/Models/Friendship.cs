

namespace MiniBook.Models
{
    public enum FriendshipStatus : byte
    {
        Pending = 0,
        Accepted = 1,
        Declined = 2
    }

    public class Friendship
    {
        public int Id { get; set; }

        // Utilisateur qui envoie la demande
        public int RequesterId { get; set; }

        // Utilisateur qui reçoit la demande
        public int AddresseeId { get; set; }

        public FriendshipStatus Status { get; set; } = FriendshipStatus.Pending;

        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;

        // Navigations
        public User Requester { get; set; } = null!;
        public User Addressee { get; set; } = null!;
    }
}