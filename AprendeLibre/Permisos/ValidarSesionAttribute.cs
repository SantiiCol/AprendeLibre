using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using AprendeLibre.Models;

namespace AprendeLibre.Permisos
{
    public class ValidarSesionAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var session = context.HttpContext.Session;

            // Verificar si la sesión contiene el usuario
            if (session.GetInt32("UsuarioId") == null)
            {
                // Redirigir al usuario a la página de inicio de sesión
                context.Result = new RedirectToActionResult("Login", "Acceso", null);
            }

            base.OnActionExecuting(context);
        }

    }
}
