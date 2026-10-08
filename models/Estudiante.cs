namespace SistemaPrestamosLibros.Models
{
    public class libro
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public bool Disponible { get; set; } = true;
    }