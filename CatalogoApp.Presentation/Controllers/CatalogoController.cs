using CatalogoApp.Application.Services;
using CatalogoApp.Domain.Models;
using Microsoft.AspNetCore.Authorization; // <-- NUEVO
using Microsoft.AspNetCore.Mvc;

namespace CatalogoApp.Presentation.Controllers
{
    public class CatalogoController : Controller
    {
        private readonly ItemService _service;

        // El servicio llega por inyección de dependencias
        public CatalogoController(ItemService service)
        {
            _service = service;
        }

        // Lista con filtro opcional por género
        public IActionResult Index(string? genero)
        {
            var items = string.IsNullOrEmpty(genero)
                ? _service.ObtenerTodos()
                : _service.ObtenerPorGenero(genero);

            ViewBag.Generos = _service.ObtenerGeneros();
            ViewBag.GeneroActual = genero;

            return View(items);
        }

        // Detalle de un item
        public IActionResult Detalle(int id)
        {
            var item = _service.ObtenerPorId(id);
            return item == null ? NotFound() : View(item);
        }

        // Formulario — GET
        public IActionResult Agregar()
        {
            return View();
        }

        // Formulario — POST
        [HttpPost]
        public IActionResult Agregar(Item item)
        {
            _service.Agregar(item);
            return RedirectToAction("Index");
        }

        // Eliminar
        public IActionResult Eliminar(int id)
        {
            _service.Eliminar(id);
            return RedirectToAction("Index");
        }

        // NUEVO: Guardar una opinión
        [HttpPost]
        [Authorize] // Bloquea la acción si no hay sesión iniciada
        public IActionResult AgregarResena(int itemId, int puntuacion, string comentario)
        {
            var item = _service.ObtenerPorId(itemId);

            if (item != null)
            {
                var nuevaResena = new Resena
                {
                    Usuario = User.Identity?.Name ?? "Anónimo", // Obtiene el nombre del usuario de la cookie
                    Puntuacion = puntuacion,
                    Comentario = comentario
                };

                item.Resenas.Add(nuevaResena);
                _service.Actualizar(item); // Guarda los cambios en el JSON
            }

            return RedirectToAction("Detalle", new { id = itemId });
        }
    }
}