using System.Collections.Generic;
using CatalogoApp.Domain.Models;

namespace CatalogoApp.Domain.Interfaces
{
    public interface IItemRepository
    {
        List<Item> ObtenerTodos();
        Item? ObtenerPorId(int id);
        void Agregar(Item item);
        void Actualizar(Item item); // <-- Aquí está el nuevo método que faltaba
        void Eliminar(int id);
    }
}