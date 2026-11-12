
using SistemaPrestamosLibros.Domain.Core;

namespace SistemaPrestamosLibros.Domain.Entities
{
    public class Devolucion : BaseEntity
    {
        public int PrestamoId { get; set; }
        public DateTime FechaDevolucionReal { get; set; } = DateTime.Now;
        public decimal MultaMora { get; set; }

        public Prestamo? Prestamo { get; set; }
    }
}
