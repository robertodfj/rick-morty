using System.ComponentModel.DataAnnotations;

namespace RickYMorty.model
{
    public class Character
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Status { get; set; }
        public required string Species { get; set; }
        public required string Gender { get; set; }
    }
}