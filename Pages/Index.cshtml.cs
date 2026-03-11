using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace RazorPages.Pages
{
    public class IndexModel : PageModel
    {

        private readonly IWebHostEnvironment _env;

        public IndexModel(IWebHostEnvironment env)
        {
            _env = env;
        }

        public List<string> Imagenes { get; set; } = new();

        [BindProperty]
        public IFormFile Imagen { get; set; }

        public void OnGet()
        {

            string ruta = Path.Combine(_env.WebRootPath, "uploads");

            if (Directory.Exists(ruta))
            {
                Imagenes = Directory.GetFiles(ruta)
                    .Select(x => Path.GetFileName(x))
                    .ToList();
            }

        }

        public async Task<IActionResult> OnPostAsync()
        {

            var captchaResponse = Request.Form["g-recaptcha-response"];

            if (string.IsNullOrEmpty(captchaResponse))
            {
                ModelState.AddModelError("", "Debes completar el reCAPTCHA");
                return Page();
            }

            var secretKey = "6Lf01kksAAAAADm7HEKCGTVVZqfqgfp3hTYvRmwW";

            using var client = new HttpClient();

            var response = await client.PostAsync(
                $"https://www.google.com/recaptcha/api/siteverify?secret={secretKey}&response={captchaResponse}",
                null);

            var jsonString = await response.Content.ReadAsStringAsync();

            var captchaResult = JsonDocument.Parse(jsonString);

            bool success = captchaResult.RootElement.GetProperty("success").GetBoolean();

            if (!success)
            {
                ModelState.AddModelError("", "Verifica que no eres un robot");
                return Page();
            }

            if (Imagen != null)
            {

                string carpeta = Path.Combine(_env.WebRootPath, "uploads");

                if (!Directory.Exists(carpeta))
                {
                    Directory.CreateDirectory(carpeta);
                }

                string ruta = Path.Combine(carpeta, Imagen.FileName);

                using var stream = new FileStream(ruta, FileMode.Create);

                await Imagen.CopyToAsync(stream);

            }

            return RedirectToPage();

        }
    }
}