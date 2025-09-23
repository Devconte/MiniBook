using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
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
        
        // Pour les requêtes API, retourner 401 au lieu de rediriger
        options.Events.OnRedirectToLogin = context =>
        {
            if (context.Request.Path.StartsWithSegments("/api"))
            {
                context.Response.StatusCode = 401;
                return Task.CompletedTask;
            }
            context.Response.Redirect(context.RedirectUri);
            return Task.CompletedTask;
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200", "http://localhost:5001")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials(); // Important pour les cookies d'authentification
    });
});

// MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

// --- Pipeline HTTP ---
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// === Seed des données (en arrière-plan) ===
_ = Task.Run(async () =>
{
    await Task.Delay(5000); // Laisser l'API démarrer d'abord
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        DbInitializer.Seed(db);
        Console.WriteLine("✅ DB initialisée");
    }
    catch 
    {
        Console.WriteLine("⚠️ DB pas encore prête");
    }
});

app.UseHttpsRedirection();

// 🔧 CORS avant UseRouting pour être plus efficace
app.UseCors("AllowAngular");

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

// === Message de démarrage personnalisé ===
app.Lifetime.ApplicationStarted.Register(() =>
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    var addresses = app.Services.GetRequiredService<IServer>().Features.Get<IServerAddressesFeature>()?.Addresses;
    
    Console.WriteLine();
    Console.WriteLine("🚀 ===============================================");
    Console.WriteLine("🎉 MiniBook API est maintenant disponible !");
    Console.WriteLine("🚀 ===============================================");
    
    if (addresses != null)
    {
        foreach (var address in addresses)
        {
            Console.WriteLine($"📍 Application Web : {address}");
            Console.WriteLine($"🔗 API Posts      : {address}/api/posts");
        }
    }
    
    Console.WriteLine("🗄️  Base de données : SQL Server (Docker) - localhost:1433");
    Console.WriteLine("📋 Collection Postman : MiniBook-API-Simple.postman_collection.json");
    Console.WriteLine();
    Console.WriteLine("💡 Endpoints disponibles :");
    Console.WriteLine("   GET    /api/posts        - Lister tous les posts");
    Console.WriteLine("   GET    /api/posts/{id}   - Récupérer un post");
    Console.WriteLine("   POST   /api/posts        - Créer un post");
    Console.WriteLine("   PUT    /api/posts/{id}   - Modifier un post");
    Console.WriteLine("   DELETE /api/posts/{id}   - Supprimer un post (Admin)");
    Console.WriteLine();
    Console.WriteLine("🔐 Note : Tous les endpoints nécessitent une authentification");
    Console.WriteLine("🔐 CORS : Autorisé pour Angular (ports 4200, 64541, 4201)");
    Console.WriteLine("🚀 ===============================================");
    Console.WriteLine();
});

app.Run();