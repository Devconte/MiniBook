using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniBook.Data;
using System.Security.Claims;

namespace MiniBook.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _db;

        public AccountController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = _db.Users.FirstOrDefault(u => u.Email == email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                ModelState.AddModelError("", "Invalid email or password");
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role) // "User" ou "Admin"
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return RedirectToAction("Index", "Post"); // redirige après login
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        // API endpoint pour Angular - récupérer l'utilisateur actuel
        [HttpGet]
        [Route("api/account/current")]
        [Authorize]
        public IActionResult GetCurrentUser()
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            if (userEmail == null) return Unauthorized();
            var user = _db.Users.FirstOrDefault(u => u.Email == userEmail);
            if (user == null) return Unauthorized();

            return Ok(new
            {
                id = user.Id,
                userName = user.UserName,
                email = user.Email,
                role = user.Role
            });
        }

        // API endpoint pour Angular - login
        [HttpPost]
        [Route("api/account/login")]
        public async Task<IActionResult> ApiLogin([FromBody] LoginApiRequest request)
        {
            var user = _db.Users.FirstOrDefault(u => u.Email == request.email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.password, user.PasswordHash))
            {
                return Unauthorized(new { message = "Invalid email or password" });
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return Ok(new
            {
                id = user.Id,
                userName = user.UserName,
                email = user.Email,
                role = user.Role
            });
        }

        public class LoginApiRequest
        {
            public string email { get; set; } = string.Empty;
            public string password { get; set; } = string.Empty;
        }
    }
}