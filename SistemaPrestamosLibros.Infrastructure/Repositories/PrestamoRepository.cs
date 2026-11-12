
using Microsoft.EntityFrameworkCore;
using SistemaPrestamosLibros.Domain.Entities;
using SistemaPrestamosLibros.Infrastructure.Context;

namespace SistemaPrestamosLibros.Infrastructure.Repositories
{
    public class PrestamoRepository : GenericRepository<Prestamo>
    {
        public PrestamoRepository(DataContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Prestamo>> ObtenerActivosAsync()
        {
            return await _context.Prestamos
                .Include(p => p.Libro)
                .Include(p => p.Estudiante)
                .Where(p => p.Activo)
                .ToListAsync();
        }

        public async Task<IEnumerable<Prestamo>> ObtenerPorEstudianteAsync(int estudianteId)
        {
            return await _context.Prestamos
                .Include(p => p.Libro)
                .Where(p => p.EstudianteId == estudianteId)
                .ToListAsync();
        }
    }
}
