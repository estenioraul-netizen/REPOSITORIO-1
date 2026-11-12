
using Microsoft.AspNetCore.Mvc;
using SistemaPrestamosLibros.Infrastructure.Repositories;
using SistemaPrestamosLibros.Models;
using PrestamoEntity = SistemaPrestamosLibros.Domain.Entities.Prestamo;
using LibroEntity = SistemaPrestamosLibros.Domain.Entities.Libro;
using EstudianteEntity = SistemaPrestamosLibros.Domain.Entities.Estudiante;

namespace SistemaPrestamosLibros.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrestamosController : ControllerBase
    {
        private readonly IGenericRepository<PrestamoEntity> _prestamos;
        private readonly IGenericRepository<LibroEntity> _libros;
        private readonly IGenericRepository<EstudianteEntity> _estudiantes;

        public PrestamosController(
            IGenericRepository<PrestamoEntity> prestamos,
            IGenericRepository<LibroEntity> libros,
            IGenericRepository<EstudianteEntity> estudiantes)
        {
            _prestamos = prestamos;
            _libros = libros;
            _estudiantes = estudiantes;
        }

        // GET: api/Prestamos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PrestamoDTO>>> GetPrestamos()
        {
            var prestamos = await _prestamos.GetAllAsync();

            var prestamosDTO = prestamos.Select(prestamo => new PrestamoDTO
            {
                Id = prestamo.Id,
                LibroId = prestamo.LibroId,
                EstudianteId = prestamo.EstudianteId,
                FechaPrestamo = prestamo.FechaPrestamo,
                FechaLimite = prestamo.FechaLimite,
                Activo = prestamo.Activo
            }).ToList();

            return Ok(prestamosDTO);
        }

        // GET: api/Prestamos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PrestamoDTO>> GetPrestamo(int id)
        {
            var prestamo = await _prestamos.GetByIdAsync(id);

            if (prestamo == null)
                return NotFound();

            var prestamoDTO = new PrestamoDTO
            {
                Id = prestamo.Id,
                LibroId = prestamo.LibroId,
                EstudianteId = prestamo.EstudianteId,
                FechaPrestamo = prestamo.FechaPrestamo,
                FechaLimite = prestamo.FechaLimite,
                Activo = prestamo.Activo
            };

            return Ok(prestamoDTO);
        }

        // POST: api/Prestamos
        [HttpPost]
        public async Task<ActionResult<PrestamoDTO>> PostPrestamo(
            PrestamoDTO prestamoDTO)
        {
            var libro = await _libros.GetByIdAsync(prestamoDTO.LibroId);
            var estudiante = await _estudiantes.GetByIdAsync(prestamoDTO.EstudianteId);

            if (libro == null)
                return BadRequest("El libro no existe.");

            if (estudiante == null)
                return BadRequest("El estudiante no existe.");

            if (!libro.Disponible)
                return Conflict("El libro no está disponible.");

            var prestamo = new PrestamoEntity
            {
                LibroId = prestamoDTO.LibroId,
                EstudianteId = prestamoDTO.EstudianteId,
                FechaPrestamo = DateTime.Now,
                FechaLimite = DateTime.Now.AddDays(7),
                Activo = true
            };

            libro.Disponible = false;

            _libros.Update(libro);
            await _libros.SaveChangesAsync();

            await _prestamos.AddAsync(prestamo);
            await _prestamos.SaveChangesAsync();

            var resultadoDTO = new PrestamoDTO
            {
                Id = prestamo.Id,
                LibroId = prestamo.LibroId,
                EstudianteId = prestamo.EstudianteId,
                FechaPrestamo = prestamo.FechaPrestamo,
                FechaLimite = prestamo.FechaLimite,
                Activo = prestamo.Activo
            };

            return CreatedAtAction(
                nameof(GetPrestamo),
                new { id = prestamo.Id },
                resultadoDTO
            );
        }

        // PUT: api/Prestamos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPrestamo(
            int id, PrestamoDTO prestamoDTO)
        {
            if (id != prestamoDTO.Id)
                return BadRequest();

            var existente = await _prestamos.GetByIdAsync(id);

            if (existente == null)
                return NotFound();

            existente.FechaLimite = prestamoDTO.FechaLimite;

            _prestamos.Update(existente);
            await _prestamos.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Prestamos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePrestamo(int id)
        {
            var prestamo = await _prestamos.GetByIdAsync(id);

            if (prestamo == null)
                return NotFound();

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

            return NoContent();
        }
    }
}
