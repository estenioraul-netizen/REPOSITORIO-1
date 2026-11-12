
using SistemaPrestamosLibros.Domain.Core;

namespace SistemaPrestamosLibros.Domain.Entities
{
    public class Libro : BaseEntity
    {
        public string Titulo { get; set; } = string.Empty;
        public string Autor { get; set; } = string.Empty;
        public bool Disponible { get; set; } = true;

        public ICollection<Prestamo> Prestamos { get; set; }
            = new List<Prestamo>();
    }
}
