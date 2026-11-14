
using Microsoft.AspNetCore.Mvc;
using SistemaPrestamosLibros.Application.Contract;
using SistemaPrestamosLibros.Application.Dtos;


namespace proyecto_tarea_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevolucionesController : ControllerBase
    {
        private readonly IDevolucionService _service;

        public DevolucionesController(IDevolucionService service)
        {
            _service = service;
        }

        // GET: api/Devoluciones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DevolucionDTO>>> GetAll()
        {
            var devoluciones = await _service.GetAllAsync();
            return Ok(devoluciones);
        }

        // GET: api/Devoluciones/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DevolucionDTO>> GetById(int id)
        {
            var devolucion = await _service.GetByIdAsync(id);

            if (devolucion == null)
                return NotFound("La devolución no existe.");

            return Ok(devolucion);
        }

        // POST: api/Devoluciones
        [HttpPost]
        public async Task<ActionResult<DevolucionDTO>> Create(
            DevolucionDTO dto)
        {
            try
            {
                var devolucion = await _service.CreateAsync(dto);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = devolucion.Id },
                    devolucion
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

        // DELETE: api/Devoluciones/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var eliminado = await _service.DeleteAsync(id);

            if (!eliminado)
                return NotFound("La devolución no existe.");

            return NoContent();
        }
    }
}
