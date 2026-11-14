using SistemaPrestamosLibros.Application.Dtos;

namespace SistemaPrestamosLibros.Application.Contract
{
    public interface IPrestamoService
    {
        Task<IEnumerable<PrestamoDTO>> GetAllAsync();

        Task<PrestamoDTO?> GetByIdAsync(int id);

        Task<PrestamoDTO> CreateAsync(PrestamoDTO prestamo);

        Task<bool> UpdateAsync(int id, PrestamoDTO prestamo);

        Task<bool> DeleteAsync(int id);
    }
}