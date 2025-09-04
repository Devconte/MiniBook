using System.ComponentModel.DataAnnotations;

namespace MiniBook.Models
{
    public class Comment
    {
        public int Id { get; set; }

        // FK vers Post
        public int PostId { get; set; }

        // FK vers User (auteur)
        public int AuthorId { get; set; }

        [Required, StringLength(1000)]
        public string Content { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigations
        public Post Post { get; set; } = null!;
        public User Author { get; set; } = null!;
    }
}
