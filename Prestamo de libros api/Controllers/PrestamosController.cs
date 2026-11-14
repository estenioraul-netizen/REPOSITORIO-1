
using Microsoft.AspNetCore.Mvc;
using SistemaPrestamosLibros.Application.Contract;
using SistemaPrestamosLibros.Application.Dtos;

namespace proyecto_tarea_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrestamosController : ControllerBase
    {
        private readonly IPrestamoService _service;

        public PrestamosController(IPrestamoService service)
        {
            _service = service;
        }

        // GET: api/Prestamos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PrestamoDTO>>> GetAll()
        {
            var prestamos = await _service.GetAllAsync();
            return Ok(prestamos);
        }

        // GET: api/Prestamos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PrestamoDTO>> GetById(int id)
        {
            var prestamo = await _service.GetByIdAsync(id);

            if (prestamo == null)
                return NotFound("El préstamo no existe.");

            return Ok(prestamo);
        }

        // POST: api/Prestamos
        [HttpPost]
        public async Task<ActionResult<PrestamoDTO>> Create(
            PrestamoDTO dto)
        {
            try
            {
                var prestamo = await _service.CreateAsync(dto);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = prestamo.Id },
                    prestamo
                );
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/Prestamos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            PrestamoDTO dto)
        {
            try
            {
                var actualizado = await _service.UpdateAsync(id, dto);

                if (!actualizado)
                    return NotFound("El préstamo no existe.");

                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/Prestamos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var eliminado = await _service.DeleteAsync(id);

            if (!eliminado)
                return NotFound("El préstamo no existe.");

            return NoContent();
        }
    }
}
