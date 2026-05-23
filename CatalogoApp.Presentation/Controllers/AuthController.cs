using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CatalogoApp.Presentation.Controllers
{
    public class AuthController : Controller
    {
        // Muestra la vista del formulario de Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // Procesa los datos del Login
        [HttpPost]
        public async Task<IActionResult> Login(string nombre, string password)
        {
            // Verificación básica: solo confirmamos que no estén vacíos.
            if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Por favor ingresa un nombre y una contraseña.";
                return View();
            }

            // Creamos la "credencial" del usuario para la cookie
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, nombre) // Guardamos el nombre para saber quién opina
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            // Iniciamos la sesión
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return RedirectToAction("Index", "Catalogo");
        }

        // Cerrar sesión
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Catalogo");
        }
    }
}