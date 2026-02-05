using System.ComponentModel.DataAnnotations;

namespace RickYMorty.dto
{
    public class CharacterResponse
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(50)]
        public string Status { get; set; } = null!;

        [Required]
        [StringLength(50)]
        public string Species { get; set; } = null!;

        [StringLength(100)]
        public string? Type { get; set; }

        [Required]
        [StringLength(50)]
        public string Gender { get; set; } = null!;
    }
}