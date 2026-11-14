using System.ComponentModel.DataAnnotations;

namespace SistemaPrestamosLibros.Application.Dtos
{
    public class EstudianteDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, MinimumLength = 2)]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "La matrícula es obligatoria.")]
        [StringLength(30, MinimumLength = 3)]
        public string Matricula { get; set; } = string.Empty;
    }
}