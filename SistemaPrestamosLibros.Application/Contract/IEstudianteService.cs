using SistemaPrestamosLibros.Application.Dtos;

namespace SistemaPrestamosLibros.Application.Contract
{
    public interface IEstudianteService
    {
        Task<IEnumerable<EstudianteDTO>> GetAllAsync();

        Task<EstudianteDTO?> GetByIdAsync(int id);

        Task<EstudianteDTO> CreateAsync(EstudianteDTO estudiante);

        Task<bool> UpdateAsync(int id, EstudianteDTO estudiante);

        Task<bool> DeleteAsync(int id);
    }
}