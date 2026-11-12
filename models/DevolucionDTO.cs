namespace SistemaPrestamosLibros.Models
{
    public class DevolucionDTO
    {
        public int Id { get; set; }
        public int PrestamoId { get; set; }
        public DateTime FechaDevolucionReal { get; set; }
        public decimal MultaMora { get; set; }
    }
}