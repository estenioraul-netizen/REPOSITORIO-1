namespace SistemaPrestamosLibros.Models
{
    public class PrestamoDTO
    {
        public int Id { get; set; }
        public int LibroId { get; set; }
        public int EstudianteId { get; set; }
        public DateTime FechaPrestamo { get; set; }
        public DateTime FechaLimite { get; set; }
        public bool Activo { get; set; }
    }
}