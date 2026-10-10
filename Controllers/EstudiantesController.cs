using Microsoft.AspNetCore.Mvc;

namespace proyecto_tarea_1.Controllers
{
    public class EstudiantesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
