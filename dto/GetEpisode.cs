using System.ComponentModel.DataAnnotations;

namespace RickYMorty.dto
{
    public class GetEpisode
    {
        [Required]
        [Range(1, 51, ErrorMessage = "Id must be between 1 and 51.")]
        public int Id { get; set; }

        public GetEpisode(int id)
        {
            Id = id;
        }
    }
}