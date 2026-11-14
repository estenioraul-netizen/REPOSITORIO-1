using System.ComponentModel.DataAnnotations;

namespace SistemaPrestamosLibros.Application.Dtos
{
    public class PrestamoDTO
    {
        public int Id { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un libro válido.")]
        public int LibroId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un estudiante válido.")]
        public int EstudianteId { get; set; }

        public DateTime FechaPrestamo { get; set; }

        public DateTime FechaLimite { get; set; }

        public bool Activo { get; set; }
    }
}