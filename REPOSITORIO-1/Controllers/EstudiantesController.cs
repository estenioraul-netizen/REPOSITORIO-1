
using Microsoft.AspNetCore.Mvc;
using SistemaPrestamosLibros.Infrastructure.Repositories;
using SistemaPrestamosLibros.Models;
using EstudianteEntity = SistemaPrestamosLibros.Domain.Entities.Estudiante;

namespace SistemaPrestamosLibros.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstudiantesController : ControllerBase
    {
        private readonly IGenericRepository<EstudianteEntity> _repository;

        public EstudiantesController(
            IGenericRepository<EstudianteEntity> repository)
        {
            _repository = repository;
        }

        // GET: api/Estudiantes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EstudianteDTO>>> GetEstudiantes()
        {
            var estudiantes = await _repository.GetAllAsync();

            var estudiantesDTO = estudiantes.Select(estudiante => new EstudianteDTO
            {
                Id = estudiante.Id,
                Nombre = estudiante.Nombre,
                Matricula = estudiante.Matricula
            }).ToList();

            return Ok(estudiantesDTO);
        }

        // GET: api/Estudiantes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EstudianteDTO>> GetEstudiante(int id)
        {
            var estudiante = await _repository.GetByIdAsync(id);

            if (estudiante == null)
                return NotFound();

            var estudianteDTO = new EstudianteDTO
            {
                Id = estudiante.Id,
                Nombre = estudiante.Nombre,
                Matricula = estudiante.Matricula
            };

            return Ok(estudianteDTO);
        }

        // POST: api/Estudiantes
        [HttpPost]
        public async Task<ActionResult<EstudianteDTO>> PostEstudiante(
            EstudianteDTO estudianteDTO)
        {
            var estudiante = new EstudianteEntity
            {
                Nombre = estudianteDTO.Nombre,
                Matricula = estudianteDTO.Matricula
            };

            await _repository.AddAsync(estudiante);
            await _repository.SaveChangesAsync();

            var resultadoDTO = new EstudianteDTO
            {
                Id = estudiante.Id,
                Nombre = estudiante.Nombre,
                Matricula = estudiante.Matricula
            };

            return CreatedAtAction(
                nameof(GetEstudiante),
                new { id = estudiante.Id },
                resultadoDTO
            );
        }

        // PUT: api/Estudiantes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEstudiante(
            int id, EstudianteDTO estudianteDTO)
        {
            if (id != estudianteDTO.Id)
                return BadRequest();

            var estudianteExistente =
                await _repository.GetByIdAsync(id);

            if (estudianteExistente == null)
                return NotFound();

            estudianteExistente.Nombre = estudianteDTO.Nombre;
            estudianteExistente.Matricula = estudianteDTO.Matricula;

            _repository.Update(estudianteExistente);
            await _repository.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Estudiantes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEstudiante(int id)
        {
            var estudiante = await _repository.GetByIdAsync(id);

            if (estudiante == null)
                return NotFound();

            _repository.Delete(estudiante);
            await _repository.SaveChangesAsync();

            return NoContent();
        }
    }
}
