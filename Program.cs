using RazorPages.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

// Razor Pages
builder.Services.AddRazorPages();

// Obtener DATABASE_URL de Railway
var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

// Convertirla al formato que Npgsql entiende
var connectionString = new NpgsqlConnectionStringBuilder(databaseUrl)
{
    SslMode = SslMode.Require,
    TrustServerCertificate = true
}.ToString();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

app.MapRazorPages();

// Aplicar migraciones automáticamente
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.Run();