using Microsoft.EntityFrameworkCore;
// ⚠️ Remplace le namespace ci-dessous par celui où se trouve ton AppDbContext
using MiniBook.Data;

var builder = WebApplication.CreateBuilder(args);

// --- EF Core (SQL Server) ---
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

// MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

// --- Pipeline HTTP ---
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); // HSTS (prod)
}

app.UseHttpsRedirection();

// Nouveau mécanisme "Static Assets" (.NET 9 template)
app.MapStaticAssets();

app.UseRouting();

app.UseAuthorization();

// --- Routing MVC ---
// Si tu veux démarrer sur Post/Index : remplace Home par Post
app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();