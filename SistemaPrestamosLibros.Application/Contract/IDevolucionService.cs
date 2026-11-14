using SistemaPrestamosLibros.Application.Dtos;

namespace SistemaPrestamosLibros.Application.Contract
{
    public interface IDevolucionService
    {
        Task<IEnumerable<DevolucionDTO>> GetAllAsync();

        Task<DevolucionDTO?> GetByIdAsync(int id);

        Task<DevolucionDTO> CreateAsync(DevolucionDTO devolucion);

        Task<bool> DeleteAsync(int id);
    }
}