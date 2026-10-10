using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaPrestamosLibros.Data;
using SistemaPrestamosLibros.Models;

namespace SistemaPrestamosLibros.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstudiantesController : ControllerBase
    {
        private readonly AppDbContext _context;

        // Inyectamos la base de datos en el controlador
        public EstudiantesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Estudiantes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Estudiante>>> GetEstudiantes()
        {
            return await _context.Estudiantes.ToListAsync();
        }

        // GET: api/Estudiantes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Estudiante>> GetEstudiante(int id)
        {
            var estudiante = await _context.Estudiantes.FindAsync(id);
            if (estudiante == null) return NotFound();
            return estudiante;
        }

        // POST: api/Estudiantes
        [HttpPost]
        public async Task<ActionResult<Estudiante>> PostEstudiante(Estudiante estudiante)
        {
            _context.Estudiantes.Add(estudiante);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetEstudiante), new { id = estudiante.Id }, estudiante);
        }

        // PUT: api/Estudiantes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEstudiante(int id, Estudiante estudiante)
        {
            if (id != estudiante.Id) return BadRequest();

            _context.Entry(estudiante).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Estudiantes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEstudiante(int id)
        {
            var estudiante = await _context.Estudiantes.FindAsync(id);
            if (estudiante == null) return NotFound();

            _context.Estudiantes.Remove(estudiante);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}