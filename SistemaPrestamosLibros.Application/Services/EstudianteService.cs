
using SistemaPrestamosLibros.Application.Contract;
using SistemaPrestamosLibros.Application.Dtos;
using SistemaPrestamosLibros.Infrastructure.Repositories;
using EstudianteEntity = SistemaPrestamosLibros.Domain.Entities.Estudiante;

namespace SistemaPrestamosLibros.Application.Services
{
    public class EstudianteService : IEstudianteService
    {
        private readonly IGenericRepository<EstudianteEntity> _repository;

        public EstudianteService(
            IGenericRepository<EstudianteEntity> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<EstudianteDTO>> GetAllAsync()
        {
            var estudiantes = await _repository.GetAllAsync();

            return estudiantes.Select(estudiante => new EstudianteDTO
            {
                Id = estudiante.Id,
                Nombre = estudiante.Nombre,
                Matricula = estudiante.Matricula
            }).ToList();
        }

        public async Task<EstudianteDTO?> GetByIdAsync(int id)
        {
            var estudiante = await _repository.GetByIdAsync(id);

            if (estudiante == null)
                return null;

            return new EstudianteDTO
            {
                Id = estudiante.Id,
                Nombre = estudiante.Nombre,
                Matricula = estudiante.Matricula
            };
        }

        public async Task<EstudianteDTO> CreateAsync(
            EstudianteDTO estudianteDTO)
        {
            ValidarEstudiante(estudianteDTO);

            var estudiantes = await _repository.GetAllAsync();

            if (estudiantes.Any(e =>
                e.Matricula.Equals(
                    estudianteDTO.Matricula.Trim(),
                    StringComparison.OrdinalIgnoreCase)))
            {
                throw new ArgumentException(
                    "Ya existe un estudiante con esa matrícula."
                );
            }

            var estudiante = new EstudianteEntity
            {
                Nombre = estudianteDTO.Nombre.Trim(),
                Matricula = estudianteDTO.Matricula.Trim()
            };

            await _repository.AddAsync(estudiante);
            await _repository.SaveChangesAsync();

            return new EstudianteDTO
            {
                Id = estudiante.Id,
                Nombre = estudiante.Nombre,
                Matricula = estudiante.Matricula
            };
        }

        public async Task<bool> UpdateAsync(
            int id,
            EstudianteDTO estudianteDTO)
        {
            if (id != estudianteDTO.Id)
                throw new ArgumentException(
                    "El ID de la URL no coincide con el ID del estudiante."
                );

            ValidarEstudiante(estudianteDTO);

            var estudiante = await _repository.GetByIdAsync(id);

            if (estudiante == null)
                return false;

            var estudiantes = await _repository.GetAllAsync();

            if (estudiantes.Any(e =>
                e.Id != id &&
                e.Matricula.Equals(
                    estudianteDTO.Matricula.Trim(),
                    StringComparison.OrdinalIgnoreCase)))
            {
                throw new ArgumentException(
                    "Ya existe otro estudiante con esa matrícula."
                );
            }

            estudiante.Nombre = estudianteDTO.Nombre.Trim();
            estudiante.Matricula = estudianteDTO.Matricula.Trim();

            _repository.Update(estudiante);
            await _repository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var estudiante = await _repository.GetByIdAsync(id);

            if (estudiante == null)
                return false;

            _repository.Delete(estudiante);
            await _repository.SaveChangesAsync();

            return true;
        }

        private static void ValidarEstudiante(
            EstudianteDTO estudiante)
        {
            if (estudiante == null)
                throw new ArgumentNullException(nameof(estudiante));

            if (string.IsNullOrWhiteSpace(estudiante.Nombre))
                throw new ArgumentException(
                    "El nombre del estudiante es obligatorio."
                );

            if (estudiante.Nombre.Trim().Length < 2 ||
                estudiante.Nombre.Trim().Length > 100)
                throw new ArgumentException(
                    "El nombre debe tener entre 2 y 100 caracteres."
                );

            if (string.IsNullOrWhiteSpace(estudiante.Matricula))
                throw new ArgumentException(
                    "La matrícula es obligatoria."
                );

            if (estudiante.Matricula.Trim().Length < 3 ||
                estudiante.Matricula.Trim().Length > 30)
                throw new ArgumentException(
                    "La matrícula debe tener entre 3 y 30 caracteres."
                );
        }
    }
}
