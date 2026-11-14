
using Microsoft.AspNetCore.Mvc;
using SistemaPrestamosLibros.Application.Contract;
using SistemaPrestamosLibros.Application.Dtos;

namespace proyecto_tarea_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstudiantesController : ControllerBase
    {
        private readonly IEstudianteService _service;

        public EstudiantesController(IEstudianteService service)
        {
            _service = service;
        }

        // GET: api/Estudiantes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EstudianteDTO>>> GetAll()
        {
            var estudiantes = await _service.GetAllAsync();
            return Ok(estudiantes);
        }

        // GET: api/Estudiantes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EstudianteDTO>> GetById(int id)
        {
            var estudiante = await _service.GetByIdAsync(id);

            if (estudiante == null)
                return NotFound("El estudiante no existe.");

            return Ok(estudiante);
        }

        // POST: api/Estudiantes
        [HttpPost]
        public async Task<ActionResult<EstudianteDTO>> Create(
            EstudianteDTO dto)
        {
            try
            {
                var estudiante = await _service.CreateAsync(dto);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = estudiante.Id },
                    estudiante
                );
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/Estudiantes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            EstudianteDTO dto)
        {
            try
            {
                var actualizado = await _service.UpdateAsync(id, dto);

                if (!actualizado)
                    return NotFound("El estudiante no existe.");

                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/Estudiantes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var eliminado = await _service.DeleteAsync(id);

            if (!eliminado)
                return NotFound("El estudiante no existe.");

            return NoContent();
        }
    }
}
