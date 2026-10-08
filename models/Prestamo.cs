namespace SistemaPrestamosLibros.Models
{
    public class Estudiante
    {
        public int Id { get; set; }
        public int LibvroId { get; set; }
        public int EstudianteId { get; set; }
        public DateTime FechaPrestamo { get; set; } = DateTime.Now;
        public DateTime Fechalimite { get; set; } = DateTime.Now.AddDays(7);

    }
}