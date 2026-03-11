using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPages.Data;
using RazorPages.Models;

public class EditModel : PageModel
{
    private readonly AppDbContext _context;

    public EditModel(AppDbContext context)
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
        _context.Usuarios.Update(Usuario);
        _context.SaveChanges();

        return RedirectToPage("Index");
    }
}