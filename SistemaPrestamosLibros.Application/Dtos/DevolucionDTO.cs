using System.ComponentModel.DataAnnotations;

namespace SistemaPrestamosLibros.Application.Dtos
{
    public class DevolucionDTO
    {
        public int Id { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Debe indicar un préstamo válido.")]
        public int PrestamoId { get; set; }

        public DateTime FechaDevolucionReal { get; set; }

        [Range(typeof(decimal), "0", "999999999",
            ErrorMessage = "La multa no puede ser negativa.")]
        public decimal MultaMora { get; set; }
    }
}