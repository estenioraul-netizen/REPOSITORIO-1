namespace SistemaPrestamosLibros.Models
{
    public class Devolucion
    {
        public int Id { get; set; }
        public int PrestamoId { get; set; }
        public DateTime FechaDevolucionReal { get; set; } = DateTime.Now;
        public decimal MultaMora { get; set; }
    }
}