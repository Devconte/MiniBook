using Microsoft.EntityFrameworkCore;
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

app.UseAuthorization();

// --- Routing MVC ---
app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Post}/{action=Index}/{id?}") // 🚀 je mets Post par défaut
    .WithStaticAssets();

app.Run();