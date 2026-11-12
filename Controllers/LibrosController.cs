
using Microsoft.AspNetCore.Mvc;
using SistemaPrestamosLibros.Infrastructure.Repositories;
using SistemaPrestamosLibros.Models;
using LibroEntity = SistemaPrestamosLibros.Domain.Entities.Libro;

namespace SistemaPrestamosLibros.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibrosController : ControllerBase
    {
        private readonly LibroRepository _repository;

        public LibrosController(LibroRepository repository)
        {
            _repository = repository;
        }

        // GET: api/Libros
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LibroDTO>>> GetLibros()
        {
            var libros = await _repository.GetAllAsync();

            var librosDTO = libros.Select(libro => new LibroDTO
            {
                Id = libro.Id,
                Titulo = libro.Titulo,
                Autor = libro.Autor,
                Disponible = libro.Disponible
            }).ToList();

            return Ok(librosDTO);
        }

        // GET: api/Libros/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LibroDTO>> GetLibro(int id)
        {
            var libro = await _repository.GetByIdAsync(id);

            if (libro == null)
                return NotFound();

            var libroDTO = new LibroDTO
            {
                Id = libro.Id,
                Titulo = libro.Titulo,
                Autor = libro.Autor,
                Disponible = libro.Disponible
            };

            return Ok(libroDTO);
        }

        // POST: api/Libros
        [HttpPost]
        public async Task<ActionResult<LibroDTO>> PostLibro(LibroDTO libroDTO)
        {
            var libro = new LibroEntity
            {
                Titulo = libroDTO.Titulo,
                Autor = libroDTO.Autor,
                Disponible = libroDTO.Disponible
            };

            await _repository.AddAsync(libro);
            await _repository.SaveChangesAsync();

            var resultadoDTO = new LibroDTO
            {
                Id = libro.Id,
                Titulo = libro.Titulo,
                Autor = libro.Autor,
                Disponible = libro.Disponible
            };

            return CreatedAtAction(
                nameof(GetLibro),
                new { id = libro.Id },
                resultadoDTO
            );
        }

        // PUT: api/Libros/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLibro(int id, LibroDTO libroDTO)
        {
            if (id != libroDTO.Id)
                return BadRequest();

            var libroExistente = await _repository.GetByIdAsync(id);

            if (libroExistente == null)
                return NotFound();

            libroExistente.Titulo = libroDTO.Titulo;
            libroExistente.Autor = libroDTO.Autor;
            libroExistente.Disponible = libroDTO.Disponible;

            _repository.Update(libroExistente);
            await _repository.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Libros/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLibro(int id)
        {
            var libro = await _repository.GetByIdAsync(id);

            if (libro == null)
                return NotFound();

            _repository.Delete(libro);
            await _repository.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/Libros/disponibles
        [HttpGet("disponibles")]
        public async Task<ActionResult<IEnumerable<LibroDTO>>> GetLibrosDisponibles()
        {
            var libros = await _repository.ObtenerDisponiblesAsync();

            var librosDTO = libros.Select(libro => new LibroDTO
            {
                Id = libro.Id,
                Titulo = libro.Titulo,
                Autor = libro.Autor,
                Disponible = libro.Disponible
            }).ToList();

            return Ok(librosDTO);
        }

        // GET: api/Libros/buscar/titulo
        [HttpGet("buscar/{titulo}")]
        public async Task<ActionResult<IEnumerable<LibroDTO>>> BuscarPorTitulo(string titulo)
        {
            var libros = await _repository.BuscarPorTituloAsync(titulo);

            var librosDTO = libros.Select(libro => new LibroDTO
            {
                Id = libro.Id,
                Titulo = libro.Titulo,
                Autor = libro.Autor,
                Disponible = libro.Disponible
            }).ToList();

            return Ok(librosDTO);
        }
    }
}
