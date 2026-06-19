using InventoryManagementSystem.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Railway memberikan port melalui environment variable PORT.
// Aplikasi harus dapat diakses melalui 0.0.0.0.
var railwayPort = Environment.GetEnvironmentVariable("PORT");

if (!string.IsNullOrWhiteSpace(railwayPort))
{
    builder.WebHost.UseUrls($"http://0.0.0.0:{railwayPort}");
}

// Gunakan Railway Volume untuk menyimpan database SQLite.
// Saat dijalankan lokal, tetap menggunakan inventory.db di folder proyek.
var volumePath =
    Environment.GetEnvironmentVariable("RAILWAY_VOLUME_MOUNT_PATH");

var connectionString = string.IsNullOrWhiteSpace(volumePath)
    ? builder.Configuration.GetConnectionString("DefaultConnection")
        ?? "Data Source=inventory.db"
    : $"Data Source={Path.Combine(volumePath, "inventory.db")}";

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseHttpsRedirection();
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var database =
        scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    database.Database.EnsureCreated();
    DbSeeder.Seed(database);
}

app.Run();