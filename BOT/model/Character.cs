namespace Bot.model.response
{
    public class Character
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Status { get; set; } = "";
        public string Species { get; set; } = "";
        public string Gender { get; set; } = "";
        public bool ForSale { get; set; }
        public double Price { get; set; }
    }
}