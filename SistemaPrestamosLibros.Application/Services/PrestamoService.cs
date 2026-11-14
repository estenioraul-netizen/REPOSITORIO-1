
using SistemaPrestamosLibros.Application.Contract;
using SistemaPrestamosLibros.Application.Dtos;
using SistemaPrestamosLibros.Infrastructure.Repositories;
using PrestamoEntity = SistemaPrestamosLibros.Domain.Entities.Prestamo;
using LibroEntity = SistemaPrestamosLibros.Domain.Entities.Libro;
using EstudianteEntity = SistemaPrestamosLibros.Domain.Entities.Estudiante;

namespace SistemaPrestamosLibros.Application.Services
{
    public class PrestamoService : IPrestamoService
    {
        private readonly IGenericRepository<PrestamoEntity> _prestamos;
        private readonly IGenericRepository<LibroEntity> _libros;
        private readonly IGenericRepository<EstudianteEntity> _estudiantes;

        public PrestamoService(
            IGenericRepository<PrestamoEntity> prestamos,
            IGenericRepository<LibroEntity> libros,
            IGenericRepository<EstudianteEntity> estudiantes)
        {
            _prestamos = prestamos;
            _libros = libros;
            _estudiantes = estudiantes;
        }

        public async Task<IEnumerable<PrestamoDTO>> GetAllAsync()
        {
            var prestamos = await _prestamos.GetAllAsync();

            return prestamos.Select(p => MapearDTO(p)).ToList();
        }

        public async Task<PrestamoDTO?> GetByIdAsync(int id)
        {
            var prestamo = await _prestamos.GetByIdAsync(id);

            return prestamo == null ? null : MapearDTO(prestamo);
        }

        public async Task<PrestamoDTO> CreateAsync(PrestamoDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            if (dto.LibroId <= 0)
                throw new ArgumentException("Debe seleccionar un libro válido.");

            if (dto.EstudianteId <= 0)
                throw new ArgumentException("Debe seleccionar un estudiante válido.");

            var libro = await _libros.GetByIdAsync(dto.LibroId);
            if (libro == null)
                throw new ArgumentException("El libro no existe.");

            var estudiante = await _estudiantes.GetByIdAsync(dto.EstudianteId);
            if (estudiante == null)
                throw new ArgumentException("El estudiante no existe.");

            if (!libro.Disponible)
                throw new InvalidOperationException("El libro no está disponible.");

            var fechaPrestamo = DateTime.Now;
            var fechaLimite = dto.FechaLimite == default
                ? fechaPrestamo.AddDays(7)
                : dto.FechaLimite;

            if (fechaLimite <= fechaPrestamo)
                throw new ArgumentException(
                    "La fecha límite debe ser posterior a la fecha del préstamo.");

            var prestamo = new PrestamoEntity
            {
                LibroId = dto.LibroId,
                EstudianteId = dto.EstudianteId,
                FechaPrestamo = fechaPrestamo,
                FechaLimite = fechaLimite,
                Activo = true
            };

            libro.Disponible = false;

            _libros.Update(libro);
            await _libros.SaveChangesAsync();

            await _prestamos.AddAsync(prestamo);
            await _prestamos.SaveChangesAsync();

            return MapearDTO(prestamo);
        }

        public async Task<bool> UpdateAsync(int id, PrestamoDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            if (id != dto.Id)
                throw new ArgumentException("El ID de la URL no coincide con el préstamo.");

            if (dto.FechaLimite == default)
                throw new ArgumentException("La fecha límite es obligatoria.");

            var prestamo = await _prestamos.GetByIdAsync(id);

            if (prestamo == null)
                return false;

            if (dto.FechaLimite <= prestamo.FechaPrestamo)
                throw new ArgumentException(
                    "La fecha límite debe ser posterior a la fecha del préstamo.");

            prestamo.FechaLimite = dto.FechaLimite;

            _prestamos.Update(prestamo);
            await _prestamos.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var prestamo = await _prestamos.GetByIdAsync(id);

            if (prestamo == null)
                return false;

            if (prestamo.Activo)
            {
                var libro = await _libros.GetByIdAsync(prestamo.LibroId);

                if (libro != null)
                {
                    libro.Disponible = true;
                    _libros.Update(libro);
                    await _libros.SaveChangesAsync();
                }
            }

            _prestamos.Delete(prestamo);
            await _prestamos.SaveChangesAsync();

            return true;
        }

        private static PrestamoDTO MapearDTO(PrestamoEntity p)
        {
            return new PrestamoDTO
            {
                Id = p.Id,
                LibroId = p.LibroId,
                EstudianteId = p.EstudianteId,
                FechaPrestamo = p.FechaPrestamo,
                FechaLimite = p.FechaLimite,
                Activo = p.Activo
            };
        }
    }
}
