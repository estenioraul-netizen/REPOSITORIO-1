
using SistemaPrestamosLibros.Application.Contract;
using SistemaPrestamosLibros.Application.Dtos;
using SistemaPrestamosLibros.Infrastructure.Repositories;
using LibroEntity = SistemaPrestamosLibros.Domain.Entities.Libro;

namespace SistemaPrestamosLibros.Application.Services
{
    public class LibroService : ILibroService
    {
        private readonly LibroRepository _repository;

        public LibroService(LibroRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<LibroDTO>> GetAllAsync()
        {
            var libros = await _repository.GetAllAsync();

            return libros.Select(libro => new LibroDTO
            {
                Id = libro.Id,
                Titulo = libro.Titulo,
                Autor = libro.Autor,
                Disponible = libro.Disponible
            }).ToList();
        }

        public async Task<LibroDTO?> GetByIdAsync(int id)
        {
            var libro = await _repository.GetByIdAsync(id);

            if (libro == null)
                return null;

            return new LibroDTO
            {
                Id = libro.Id,
                Titulo = libro.Titulo,
                Autor = libro.Autor,
                Disponible = libro.Disponible
            };
        }

        public async Task<LibroDTO> CreateAsync(LibroDTO libroDTO)
        {
            ValidarLibro(libroDTO);

            var libro = new LibroEntity
            {
                Titulo = libroDTO.Titulo.Trim(),
                Autor = libroDTO.Autor.Trim(),
                Disponible = libroDTO.Disponible
            };

            await _repository.AddAsync(libro);
            await _repository.SaveChangesAsync();

            return new LibroDTO
            {
                Id = libro.Id,
                Titulo = libro.Titulo,
                Autor = libro.Autor,
                Disponible = libro.Disponible
            };
        }

        public async Task<bool> UpdateAsync(int id, LibroDTO libroDTO)
        {
            if (id != libroDTO.Id)
                throw new ArgumentException(
                    "El ID de la URL no coincide con el ID del libro."
                );

            ValidarLibro(libroDTO);

            var libro = await _repository.GetByIdAsync(id);

            if (libro == null)
                return false;

            libro.Titulo = libroDTO.Titulo.Trim();
            libro.Autor = libroDTO.Autor.Trim();
            libro.Disponible = libroDTO.Disponible;

            _repository.Update(libro);
            await _repository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var libro = await _repository.GetByIdAsync(id);

            if (libro == null)
                return false;

            _repository.Delete(libro);
            await _repository.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<LibroDTO>> GetAvailableAsync()
        {
            var libros = await _repository.ObtenerDisponiblesAsync();

            return libros.Select(libro => new LibroDTO
            {
                Id = libro.Id,
                Titulo = libro.Titulo,
                Autor = libro.Autor,
                Disponible = libro.Disponible
            }).ToList();
        }

        public async Task<IEnumerable<LibroDTO>> SearchByTitleAsync(string titulo)
        {
            if (string.IsNullOrWhiteSpace(titulo))
                throw new ArgumentException(
                    "Debe proporcionar un título para realizar la búsqueda."
                );

            var libros = await _repository.BuscarPorTituloAsync(titulo.Trim());

            return libros.Select(libro => new LibroDTO
            {
                Id = libro.Id,
                Titulo = libro.Titulo,
                Autor = libro.Autor,
                Disponible = libro.Disponible
            }).ToList();
        }

        private static void ValidarLibro(LibroDTO libro)
        {
            if (libro == null)
                throw new ArgumentNullException(nameof(libro));

            if (string.IsNullOrWhiteSpace(libro.Titulo))
                throw new ArgumentException("El título es obligatorio.");

            if (libro.Titulo.Trim().Length < 2 ||
                libro.Titulo.Trim().Length > 150)
                throw new ArgumentException(
                    "El título debe tener entre 2 y 150 caracteres."
                );

            if (string.IsNullOrWhiteSpace(libro.Autor))
                throw new ArgumentException("El autor es obligatorio.");

            if (libro.Autor.Trim().Length < 2 ||
                libro.Autor.Trim().Length > 100)
                throw new ArgumentException(
                    "El autor debe tener entre 2 y 100 caracteres."
                );
        }
    }
}
