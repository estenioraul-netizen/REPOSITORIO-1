
using SistemaPrestamosLibros.Application.Contract;
using SistemaPrestamosLibros.Application.Dtos;
using SistemaPrestamosLibros.Infrastructure.Repositories;
using DevolucionEntity = SistemaPrestamosLibros.Domain.Entities.Devolucion;
using PrestamoEntity = SistemaPrestamosLibros.Domain.Entities.Prestamo;
using LibroEntity = SistemaPrestamosLibros.Domain.Entities.Libro;

namespace SistemaPrestamosLibros.Application.Services
{
    public class DevolucionService : IDevolucionService
    {
        private readonly IGenericRepository<DevolucionEntity> _devoluciones;
        private readonly IGenericRepository<PrestamoEntity> _prestamos;
        private readonly IGenericRepository<LibroEntity> _libros;

        public DevolucionService(
            IGenericRepository<DevolucionEntity> devoluciones,
            IGenericRepository<PrestamoEntity> prestamos,
            IGenericRepository<LibroEntity> libros)
        {
            _devoluciones = devoluciones;
            _prestamos = prestamos;
            _libros = libros;
        }

        public async Task<IEnumerable<DevolucionDTO>> GetAllAsync()
        {
            var devoluciones = await _devoluciones.GetAllAsync();

            return devoluciones.Select(d => MapearDTO(d)).ToList();
        }

        public async Task<DevolucionDTO?> GetByIdAsync(int id)
        {
            var devolucion = await _devoluciones.GetByIdAsync(id);

            return devolucion == null ? null : MapearDTO(devolucion);
        }

        public async Task<DevolucionDTO> CreateAsync(DevolucionDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            if (dto.PrestamoId <= 0)
                throw new ArgumentException("Debe indicar un préstamo válido.");

            if (dto.MultaMora < 0)
                throw new ArgumentException("La multa no puede ser negativa.");

            var prestamo = await _prestamos.GetByIdAsync(dto.PrestamoId);

            if (prestamo == null)
                throw new ArgumentException("El préstamo no existe.");

            if (!prestamo.Activo)
                throw new InvalidOperationException("Este préstamo ya fue devuelto.");

            var libro = await _libros.GetByIdAsync(prestamo.LibroId);

            if (libro == null)
                throw new ArgumentException("No se encontró el libro asociado.");

            var devolucion = new DevolucionEntity
            {
                PrestamoId = dto.PrestamoId,
                FechaDevolucionReal = DateTime.Now,
                MultaMora = dto.MultaMora
            };

            prestamo.Activo = false;
            libro.Disponible = true;

            await _devoluciones.AddAsync(devolucion);
            _prestamos.Update(prestamo);
            _libros.Update(libro);

            await _devoluciones.SaveChangesAsync();
            await _prestamos.SaveChangesAsync();
            await _libros.SaveChangesAsync();

            return MapearDTO(devolucion);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var devolucion = await _devoluciones.GetByIdAsync(id);

            if (devolucion == null)
                return false;

            var prestamo = await _prestamos.GetByIdAsync(devolucion.PrestamoId);

            if (prestamo != null)
            {
                prestamo.Activo = true;
                _prestamos.Update(prestamo);

                var libro = await _libros.GetByIdAsync(prestamo.LibroId);

                if (libro != null)
                {
                    libro.Disponible = false;
                    _libros.Update(libro);
                }

                await _prestamos.SaveChangesAsync();
                await _libros.SaveChangesAsync();
            }

            _devoluciones.Delete(devolucion);
            await _devoluciones.SaveChangesAsync();

            return true;
        }

        private static DevolucionDTO MapearDTO(DevolucionEntity d)
        {
            return new DevolucionDTO
            {
                Id = d.Id,
                PrestamoId = d.PrestamoId,
                FechaDevolucionReal = d.FechaDevolucionReal,
                MultaMora = d.MultaMora
            };
        }
    }
}
