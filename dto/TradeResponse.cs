using System.ComponentModel.DataAnnotations;

namespace RickYMorty.dto
{
    public class TradeResponse
    {
        public int Id { get; set; }                     // ID del item
        public string Name { get; set; } = null!;       // Nombre del personaje o episodio
        public string Type { get; set; } = null!;      // "Character" o "Episode"
        public double Price { get; set; }              // Precio de venta
        public bool ForSale { get; set; }              // Si está en venta
        public string? ExtraInfo { get; set; }         // Campo opcional para info adicional (ej: Status, AirDate, etc.)
        public string? Url { get; set; }               // Para episodios, la URL
        public string[]? Characters { get; set; }      // Para episodios, los personajes que incluye
    }
}