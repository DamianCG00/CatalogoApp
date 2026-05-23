using CatalogoApp.Application.Services;
using CatalogoApp.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting; // <-- NUEVO: Para encontrar la carpeta wwwroot
using Microsoft.AspNetCore.Http; // <-- NUEVO: Para manejar IFormFile
using System.IO; // <-- NUEVO: Para manejar rutas de archivos
using System;

namespace CatalogoApp.Presentation.Controllers
{
    public class CatalogoController : Controller
    {
        private readonly ItemService _service;
        private readonly IWebHostEnvironment _env; // <-- NUEVO

        // Actualizamos el constructor para inyectar IWebHostEnvironment
        public CatalogoController(ItemService service, IWebHostEnvironment env)
        {
            _service = service;
            _env = env;
        }

        // ... (Tus métodos Index, Detalle y Agregar GET se quedan igual) ...
        public IActionResult Index(string? genero)
        {
            var items = string.IsNullOrEmpty(genero)
                ? _service.ObtenerTodos()
                : _service.ObtenerPorGenero(genero);

            ViewBag.Generos = _service.ObtenerGeneros();
            ViewBag.GeneroActual = genero;

            return View(items);
        }

        public IActionResult Detalle(int id)
        {
            var item = _service.ObtenerPorId(id);
            return item == null ? NotFound() : View(item);
        }

        public IActionResult Agregar()
        {
            return View();
        }

        // NUEVO: Método Agregar POST modificado para recibir archivos
        [HttpPost]
        public async Task<IActionResult> Agregar(Item item, IFormFile? imagenArchivo)
        {
            // Verificamos si el usuario seleccionó un archivo
            if (imagenArchivo != null && imagenArchivo.Length > 0)
            {
                // 1. Definimos la ruta: wwwroot/images/portadas
                string carpetaDestino = Path.Combine(_env.WebRootPath, "images", "portadas");

                // Si la carpeta no existe, la creamos
                if (!Directory.Exists(carpetaDestino))
                {
                    Directory.CreateDirectory(carpetaDestino);
                }

                // 2. Creamos un nombre único para la imagen (para evitar que dos juegos con una foto llamada "portada.jpg" se sobrescriban)
                string nombreArchivoUnico = Guid.NewGuid().ToString() + "_" + imagenArchivo.FileName;
                string rutaFisicaCompleta = Path.Combine(carpetaDestino, nombreArchivoUnico);

                // 3. Guardamos el archivo físicamente en la carpeta
                using (var fileStream = new FileStream(rutaFisicaCompleta, FileMode.Create))
                {
                    await imagenArchivo.CopyToAsync(fileStream);
                }

                // 4. Le decimos a nuestro Item que su ImagenUrl ahora es esta ruta local
                item.ImagenUrl = "/images/portadas/" + nombreArchivoUnico;
            }

            _service.Agregar(item);
            return RedirectToAction("Index");
        }

        public IActionResult Eliminar(int id)
        {
            _service.Eliminar(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize]
        public IActionResult AgregarResena(int itemId, int puntuacion, string comentario)
        {
            var item = _service.ObtenerPorId(itemId);

            if (item != null)
            {
                var nuevaResena = new Resena
                {
                    Usuario = User.Identity?.Name ?? "Anónimo",
                    Puntuacion = puntuacion,
                    Comentario = comentario
                };

                item.Resenas.Add(nuevaResena);
                _service.Actualizar(item);
            }

            return RedirectToAction("Detalle", new { id = itemId });
        }
    }
}