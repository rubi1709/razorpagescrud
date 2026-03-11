using RazorPages.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// AGREGAR RAZOR PAGES
builder.Services.AddRazorPages();

// CONEXIÓN A POSTGRESQL (Railway)
var connection = Environment.GetEnvironmentVariable("DATABASE_URL");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connection));

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

app.MapRazorPages();

// MIGRACIONES AUTOMÁTICAS
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.Run();