namespace CatalogoApp.Domain.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Genero { get; set; } = string.Empty;
        public string Consola { get; set; } = string.Empty;
        public int Ano { get; set; }
        public string Descripcion { get; set; } = string.Empty;

        // Nuevo campo para la URL de la portada del juego
        public string ImagenUrl { get; set; } = string.Empty;

        // Lista para guardar todas las opiniones de este juego
        public List<Resena> Resenas { get; set; } = new List<Resena>();
    }

    public class Resena
    {
        public string Usuario { get; set; } = string.Empty;
        public int Puntuacion { get; set; } // De 1 a 5 estrellas
        public string Comentario { get; set; } = string.Empty;
    }
}