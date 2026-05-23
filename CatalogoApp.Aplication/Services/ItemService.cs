using System.Collections.Generic;
using System.Linq;
using CatalogoApp.Domain.Interfaces;
using CatalogoApp.Domain.Models;

namespace CatalogoApp.Application.Services
{
    public class ItemService
    {
        private readonly IItemRepository _repository;

        public ItemService(IItemRepository repository)
        {
            _repository = repository;
        }

        public List<Item> ObtenerTodos()
        {
            return _repository.ObtenerTodos();
        }

        public Item? ObtenerPorId(int id)
        {
            return _repository.ObtenerPorId(id);
        }

        public void Agregar(Item item)
        {
            _repository.Agregar(item);
        }

        // <-- Este es el método que soluciona el error CS1061 -->
        public void Actualizar(Item item)
        {
            _repository.Actualizar(item);
        }

        public void Eliminar(int id)
        {
            _repository.Eliminar(id);
        }

        // Método para filtrar por género (usado en tu Index)
        public List<Item> ObtenerPorGenero(string genero)
        {
            var todos = _repository.ObtenerTodos();
            return todos.Where(i => i.Genero.Equals(genero, System.StringComparison.OrdinalIgnoreCase)).ToList();
        }

        // Método para obtener la lista de géneros únicos (usado en tu ViewBag.Generos)
        public List<string> ObtenerGeneros()
        {
            var todos = _repository.ObtenerTodos();
            return todos.Select(i => i.Genero).Distinct().Where(g => !string.IsNullOrEmpty(g)).ToList();
        }
    }
}