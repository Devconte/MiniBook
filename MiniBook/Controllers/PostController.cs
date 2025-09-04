
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniBook.Data;
using MiniBook.Models;

namespace MiniBook.Controllers
{
    public class PostController : Controller
    {
        private readonly AppDbContext _db;

        public PostController(AppDbContext db)
        {
            _db = db;
        }

        // GET: /Post
        // Affiche la liste des posts, auteur inclus, triés du plus récent au plus ancien
        public async Task<IActionResult> Index()
        {
            var posts = await _db.Posts
                .Include(p => p.User)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            return View(posts);
        }

        // GET: /Post/Create
        // Affiche le formulaire de création
        [HttpGet]
        public IActionResult Create()
        {
            return View(new Post());
        }

        // POST: /Post/Create
        // Valide et enregistre un nouveau post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Content")] Post post)
        {
            if (!ModelState.IsValid)
                return View(post);

            // ⚠️ En attendant l’authentification, on force un utilisateur existant.
            // Remplace 1 par l'Id d'un user présent en base (ou récupère l'Id utilisateur connecté plus tard).
            post.UserId = 1;
            post.CreatedAt = DateTime.UtcNow;

            _db.Posts.Add(post);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}