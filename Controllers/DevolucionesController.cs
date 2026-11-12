
using Microsoft.AspNetCore.Mvc;
using SistemaPrestamosLibros.Infrastructure.Repositories;
using SistemaPrestamosLibros.Models;
using DevolucionEntity = SistemaPrestamosLibros.Domain.Entities.Devolucion;
using PrestamoEntity = SistemaPrestamosLibros.Domain.Entities.Prestamo;
using LibroEntity = SistemaPrestamosLibros.Domain.Entities.Libro;

namespace SistemaPrestamosLibros.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevolucionesController : ControllerBase
    {
        private readonly IGenericRepository<DevolucionEntity> _devoluciones;
        private readonly IGenericRepository<PrestamoEntity> _prestamos;
        private readonly IGenericRepository<LibroEntity> _libros;

        public DevolucionesController(
            IGenericRepository<DevolucionEntity> devoluciones,
            IGenericRepository<PrestamoEntity> prestamos,
            IGenericRepository<LibroEntity> libros)
        {
            _devoluciones = devoluciones;
            _prestamos = prestamos;
            _libros = libros;
        }

        // GET: api/Devoluciones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DevolucionDTO>>> GetDevoluciones()
        {
            var devoluciones = await _devoluciones.GetAllAsync();

            var devolucionesDTO = devoluciones.Select(devolucion => new DevolucionDTO
            {
                Id = devolucion.Id,
                PrestamoId = devolucion.PrestamoId,
                FechaDevolucionReal = devolucion.FechaDevolucionReal,
                MultaMora = devolucion.MultaMora
            }).ToList();

            return Ok(devolucionesDTO);
        }

        // GET: api/Devoluciones/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DevolucionDTO>> GetDevolucion(int id)
        {
            var devolucion = await _devoluciones.GetByIdAsync(id);

            if (devolucion == null)
                return NotFound();

            var devolucionDTO = new DevolucionDTO
            {
                Id = devolucion.Id,
                PrestamoId = devolucion.PrestamoId,
                FechaDevolucionReal = devolucion.FechaDevolucionReal,
                MultaMora = devolucion.MultaMora
            };

            return Ok(devolucionDTO);
        }

        // POST: api/Devoluciones
        [HttpPost]
        public async Task<ActionResult<DevolucionDTO>> PostDevolucion(
            DevolucionDTO devolucionDTO)
        {
            var prestamo = await _prestamos.GetByIdAsync(devolucionDTO.PrestamoId);

            if (prestamo == null)
                return BadRequest("El préstamo no existe.");

            if (!prestamo.Activo)
                return Conflict("Este préstamo ya fue devuelto.");

            var libro = await _libros.GetByIdAsync(prestamo.LibroId);

            if (libro == null)
                return BadRequest("No se encontró el libro asociado.");

            var devolucion = new DevolucionEntity
            {
                PrestamoId = devolucionDTO.PrestamoId,
                FechaDevolucionReal = DateTime.Now,
                MultaMora = devolucionDTO.MultaMora
            };

            prestamo.Activo = false;
            libro.Disponible = true;

            await _devoluciones.AddAsync(devolucion);

            _prestamos.Update(prestamo);
            _libros.Update(libro);

            await _devoluciones.SaveChangesAsync();
            await _prestamos.SaveChangesAsync();
            await _libros.SaveChangesAsync();

            var resultadoDTO = new DevolucionDTO
            {
                Id = devolucion.Id,
                PrestamoId = devolucion.PrestamoId,
                FechaDevolucionReal = devolucion.FechaDevolucionReal,
                MultaMora = devolucion.MultaMora
            };

            return CreatedAtAction(
                nameof(GetDevolucion),
                new { id = devolucion.Id },
                resultadoDTO
            );
        }

        // DELETE: api/Devoluciones/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDevolucion(int id)
        {
            var devolucion = await _devoluciones.GetByIdAsync(id);

            if (devolucion == null)
                return NotFound();

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
                    await _libros.SaveChangesAsync();
                }

                await _prestamos.SaveChangesAsync();
            }

            _devoluciones.Delete(devolucion);
            await _devoluciones.SaveChangesAsync();

            return NoContent();
        }
    }
}
