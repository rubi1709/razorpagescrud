using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPages.Data;
using RazorPages.Models;
using Microsoft.EntityFrameworkCore;

namespace RazorPages.Pages.Usuarios
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        public IList<Usuario> Usuarios { get; set; } = new List<Usuario>();

        public int PageNumber { get; set; } = 1;
        public int TotalPages { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? Buscar { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? Desde { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? Hasta { get; set; }

        [BindProperty(SupportsGet = true)]
        public int Mostrar { get; set; } = 10;

        public async Task OnGetAsync(int pageNumber = 1)
        {
            PageNumber = pageNumber;

            var query = _context.Usuarios.AsQueryable();

            // BUSCADOR
            if (!string.IsNullOrWhiteSpace(Buscar))
            {
                query = query.Where(u =>
                    u.Nombre.Contains(Buscar) ||
                    u.Apellido.Contains(Buscar));
            }

            // DESDE
            if (Desde.HasValue)
            {
                query = query.Where(u => u.FechaRegistro >= Desde.Value);
            }

            // HASTA (incluye todo el día)
            if (Hasta.HasValue)
            {
                var fechaHasta = Hasta.Value.AddDays(1);
                query = query.Where(u => u.FechaRegistro < fechaHasta);
            }

            int totalRegistros = await query.CountAsync();

            TotalPages = (int)Math.Ceiling(totalRegistros / (double)Mostrar);

            Usuarios = await query
                .OrderByDescending(u => u.FechaRegistro)
                .Skip((PageNumber - 1) * Mostrar)
                .Take(Mostrar)
                .ToListAsync();
        }
    }
}