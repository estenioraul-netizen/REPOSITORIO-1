
using SistemaPrestamosLibros.Domain.Core;

namespace SistemaPrestamosLibros.Domain.Entities
{
    public class Estudiante : BaseEntity
    {
        public string Nombre { get; set; } = string.Empty;
        public string Matricula { get; set; } = string.Empty;

        public ICollection<Prestamo> Prestamos { get; set; }
            = new List<Prestamo>();
    }
}
