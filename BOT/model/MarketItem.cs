using System.Collections.Generic;

namespace Bot.model
{
    public class MarketItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Type { get; set; } = ""; // "Character" o "Episode"
        public int Price { get; set; }
        public bool ForSale { get; set; }
        public string? ExtraInfo { get; set; } // Por ejemplo, "S02E09" si es episodio
        public string? Url { get; set; }
        public List<string> Characters { get; set; } = new List<string>(); // URLs de personajes
    }
}