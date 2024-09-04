using Microsoft.AspNetCore.Mvc;

namespace AprendeLibre.Controllers
{
    public class FooterLinksController : Controller
    {
        public IActionResult Acerca_de_Nosotros()
        {
            return View();
        }
        public IActionResult Contactanos()
        {
            return View();
        }
        public IActionResult Terminos_y_Servicios()
        {
            return View();
        }
        public IActionResult Politica_de_Privacidad()
        {
            return View();
        }
    }
}
