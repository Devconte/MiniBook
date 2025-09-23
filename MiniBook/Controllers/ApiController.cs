using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniBook.Data;
using MiniBook.Models;

namespace MiniBook.Controllers.Api
{
    [ApiController]
    [Route("api/posts")]
    [Authorize] // 🔒 Authentification réactivée
    public class PostsController : ControllerBase
    {
        private readonly AppDbContext _db;
        public PostsController(AppDbContext db) => _db = db;

        // Endpoint de test sans base de données
        [HttpGet("test")]
        [AllowAnonymous]
        public IActionResult Test()
        {
            return Ok(new
            {
                message = "🎉 CORS et API fonctionnent !",
                timestamp = DateTime.UtcNow,
                server = "MiniBook API Docker"
            });
        }

        // GET: api/posts
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var posts = await _db.Posts.Include(p => p.User).ToListAsync();
            return Ok(posts);
        }

        // GET: api/posts/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var post = await _db.Posts.Include(p => p.User).FirstOrDefaultAsync(p => p.Id == id);
            return post == null ? NotFound() : Ok(post);
        }

        // POST: api/posts
        [HttpPost]
        public async Task<IActionResult> Create(Post post)
        {
            post.CreatedAt = DateTime.UtcNow;
            _db.Posts.Add(post);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = post.Id }, post);
        }

        // PUT: api/posts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Post updated)
        {
            var post = await _db.Posts.FindAsync(id);
            if (post == null) return NotFound();

            post.Title = updated.Title;
            post.Content = updated.Content;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/posts/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")] // ❌ Seul un admin peut supprimer
        public async Task<IActionResult> Delete(int id)
        {
            var post = await _db.Posts.FindAsync(id);
            if (post == null) return NotFound();

            _db.Posts.Remove(post);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
