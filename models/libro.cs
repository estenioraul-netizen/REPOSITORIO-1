namespace proyecto_tarea_1.Controllers
public class Libro
{
    public int Id { get; set; }
    public string Titulo { get; set; }
    public string Autor { get; set; }
    public bool Disponible { get; set; } = true;
}