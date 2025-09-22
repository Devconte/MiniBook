using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using MiniBook.Data;

var builder = WebApplication.CreateBuilder(args);

// --- EF Core (SQL Server) ---
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

// --- Auth par cookie ---
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";           // redirige si pas connecté
        options.AccessDeniedPath = "/Account/AccessDenied"; // redirige si accès refusé
    });

builder.Services.AddAuthorization();

// MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

// --- Pipeline HTTP ---
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// === Seed des données ===
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    DbInitializer.Seed(db);
}

app.UseHttpsRedirection();

// Nouveau mécanisme "Static Assets" (.NET 9 template)
app.MapStaticAssets();

app.UseRouting();

// ⚠️ Important : toujours avant UseAuthorization
app.UseAuthentication();
app.UseAuthorization();

// --- Routing MVC ---
app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Post}/{action=Index}/{id?}") // 🚀 je mets Post par défaut
    .WithStaticAssets();

app.Run();