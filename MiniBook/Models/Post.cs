using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace MiniBook.Models
{
    public class Post
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        [Required(ErrorMessage = "Le titre est obligatoire")]
        [StringLength(120)]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le contenu est obligatoire")]
        public string Content { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ValidateNever] 
        public User User { get; set; } = null!;

        [ValidateNever]
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}