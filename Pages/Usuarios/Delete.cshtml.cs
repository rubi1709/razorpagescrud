using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPages.Data;
using RazorPages.Models;

public class DeleteModel : PageModel
{
    private readonly AppDbContext _context;

    public DeleteModel(AppDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Usuario Usuario { get; set; }

    public IActionResult OnGet(int id)
    {
        Usuario = _context.Usuarios.Find(id);

        if (Usuario == null)
        {
            return RedirectToPage("Index");
        }

        return Page();
    }

    public IActionResult OnPost()
    {
        var usuario = _context.Usuarios.Find(Usuario.Id);

        if (usuario != null)
        {
            _context.Usuarios.Remove(usuario);
            _context.SaveChanges();
        }

        return RedirectToPage("Index");
    }
}