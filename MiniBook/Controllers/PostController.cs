using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniBook.Data;
using MiniBook.Models;

namespace MiniBook.Controllers
{
    public class PostController : Controller
    {
        private readonly AppDbContext _db;
        public PostController(AppDbContext db) => _db = db;

        
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var posts = await _db.Posts
                .Include(p => p.User)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            return View(posts);
        }

        
        public class PostCreateVm
        {
            [Required, StringLength(120)]
            public string Title { get; set; } = string.Empty;
            [Required]
            public string Content { get; set; } = string.Empty;
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAjax([FromForm] PostCreateVm vm)
        {
            if (!ModelState.IsValid)
            {
                var errs = ModelState.ToDictionary(
                    kv => kv.Key,
                    kv => kv.Value!.Errors.Select(e => e.ErrorMessage).ToArray());
                return BadRequest(new { ok = false, errors = errs });
            }

            var user = await _db.Users.OrderBy(u => u.Id).FirstOrDefaultAsync();
            if (user == null)
                return Problem("Aucun utilisateur en base. Vérifie le seed.");

            var post = new Post
            {
                Title = vm.Title,
                Content = vm.Content,
                UserId = user.Id,
                CreatedAt = DateTime.UtcNow
            };

            _db.Posts.Add(post);
            await _db.SaveChangesAsync();

            return Json(new
            {
                ok = true,
                post = new
                {
                    id = post.Id,
                    title = post.Title,
                    content = post.Content,
                    createdAt = post.CreatedAt,
                    author = user.UserName
                }
            });
        }
    }
}
