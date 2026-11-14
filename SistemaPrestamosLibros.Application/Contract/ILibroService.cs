using SistemaPrestamosLibros.Application.Dtos;

namespace SistemaPrestamosLibros.Application.Contract
{
    public interface ILibroService
    {
        Task<IEnumerable<LibroDTO>> GetAllAsync();

        Task<LibroDTO?> GetByIdAsync(int id);

        Task<LibroDTO> CreateAsync(LibroDTO libro);

        Task<bool> UpdateAsync(int id, LibroDTO libro);

        Task<bool> DeleteAsync(int id);

        Task<IEnumerable<LibroDTO>> GetAvailableAsync();

        Task<IEnumerable<LibroDTO>> SearchByTitleAsync(string titulo);
    }
}