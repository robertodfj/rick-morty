namespace RickYMorty.model
{
    public class Character
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Status { get; set; }
        public required string Species { get; set; }
        public required string Gender { get; set; }
        public required int OwnedByUserId { get; set; }
        public bool ForSale { get; set; } = false;
        public double Price { get; set; } = 0;
    }
}