using System.ComponentModel.DataAnnotations;

namespace RazorPages.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required]
        [StringLength(70, ErrorMessage = "El nombre no puede superar 70 caracteres")]
        public string Nombre { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "El apellido no puede superar 50 caracteres")]
        public string Apellido { get; set; }

        [Required]
        [StringLength(10)]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "El teléfono debe tener 10 dígitos")]
        public string Telefono { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "El correo no puede superar 50 caracteres")]
        [EmailAddress(ErrorMessage = "Correo no válido")]
        public string Correo { get; set; }

        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;
    }
}