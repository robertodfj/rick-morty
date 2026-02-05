using System.ComponentModel.DataAnnotations;

namespace RickYMorty.dto
{
    public class EpisodeResponse
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(50)]
        public string AirDate { get; set; } = null!;

        [Required]
        [StringLength(50)]
        public string Episode { get; set; } = null!;

        [Required]
        public required string[] Characters { get; set; }
        
        [StringLength(100)]
        public string? Url { get; set; }
        
        [StringLength(100)]
        public string? Created { get; set; }
    }
}