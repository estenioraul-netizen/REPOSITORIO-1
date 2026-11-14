
using Microsoft.EntityFrameworkCore;
using SistemaPrestamosLibros.Domain.Entities;
using SistemaPrestamosLibros.Infrastructure.Context;

namespace SistemaPrestamosLibros.Infrastructure.Repositories
{
    public class LibroRepository : GenericRepository<Libro>
    {
        public LibroRepository(DataContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Libro>> BuscarPorTituloAsync(string titulo)
        {
            return await _context.Libros
                .Where(l => l.Titulo.Contains(titulo))
                .ToListAsync();
        }

        public async Task<IEnumerable<Libro>> ObtenerDisponiblesAsync()
        {
            return await _context.Libros
                .Where(l => l.Disponible)
                .ToListAsync();
        }
    }
}
