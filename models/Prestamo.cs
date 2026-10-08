namespace SistemaPrestamosLibros.Models
{
    public class Prestamo
    {
        public int Id { get; set; }
        public int LibroId { get; set; }
        public int EstudianteId { get; set; }
        public DateTime FechaPrestamo { get; set; } = DateTime.Now;
        public DateTime FechaLimite { get; set; } = DateTime.Now.AddDays(7);
    }
}