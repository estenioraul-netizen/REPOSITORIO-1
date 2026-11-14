
using SistemaPrestamosLibros.Domain.Core;

namespace SistemaPrestamosLibros.Domain.Entities
{
    public class Prestamo : BaseEntity
    {
        public int LibroId { get; set; }
        public int EstudianteId { get; set; }

        public DateTime FechaPrestamo { get; set; } = DateTime.Now;
        public DateTime FechaLimite { get; set; } = DateTime.Now.AddDays(7);
        public bool Activo { get; set; } = true;

        public Libro? Libro { get; set; }
        public Estudiante? Estudiante { get; set; }

        public Devolucion? Devolucion { get; set; }
    }
}
