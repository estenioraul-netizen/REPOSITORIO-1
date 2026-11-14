using System.ComponentModel.DataAnnotations;

namespace SistemaPrestamosLibros.Application.Dtos
{
    public class LibroDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El título es obligatorio.")]
        [StringLength(150, MinimumLength = 2)]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El autor es obligatorio.")]
        [StringLength(100, MinimumLength = 2)]
        public string Autor { get; set; } = string.Empty;

        public bool Disponible { get; set; } = true;
    }
}