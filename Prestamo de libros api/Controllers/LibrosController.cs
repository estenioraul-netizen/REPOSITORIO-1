
using Microsoft.AspNetCore.Mvc;
using SistemaPrestamosLibros.Application.Contract;
using SistemaPrestamosLibros.Application.Dtos;


namespace proyecto_tarea_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibrosController : ControllerBase
    {
        private readonly ILibroService _service;

        public LibrosController(ILibroService service)
        {
            _service = service;
        }

        // GET: api/Libros
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LibroDTO>>> GetAll()
        {
            var libros = await _service.GetAllAsync();
            return Ok(libros);
        }

        // GET: api/Libros/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LibroDTO>> GetById(int id)
        {
            var libro = await _service.GetByIdAsync(id);

            if (libro == null)
                return NotFound("El libro no existe.");

            return Ok(libro);
        }

        // POST: api/Libros
        [HttpPost]
        public async Task<ActionResult<LibroDTO>> Create(LibroDTO dto)
        {
            try
            {
                var libro = await _service.CreateAsync(dto);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = libro.Id },
                    libro
                );
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/Libros/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, LibroDTO dto)
        {
            try
            {
                var actualizado = await _service.UpdateAsync(id, dto);

                if (!actualizado)
                    return NotFound("El libro no existe.");

                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/Libros/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var eliminado = await _service.DeleteAsync(id);

            if (!eliminado)
                return NotFound("El libro no existe.");

            return NoContent();
        }

        // GET: api/Libros/disponibles
        [HttpGet("disponibles")]
        public async Task<ActionResult<IEnumerable<LibroDTO>>> GetAvailable()
        {
            var libros = await _service.GetAvailableAsync();
            return Ok(libros);
        }

        // GET: api/Libros/buscar/Don Quijote
        [HttpGet("buscar/{titulo}")]
        public async Task<ActionResult<IEnumerable<LibroDTO>>> SearchByTitle(
            string titulo)
        {
            try
            {
                var libros = await _service.SearchByTitleAsync(titulo);
                return Ok(libros);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
