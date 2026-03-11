using RazorPages.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;

var builder = WebApplication.CreateBuilder(args);

var connection = Environment.GetEnvironmentVariable("DATABASE_URL");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connection));

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.Run();