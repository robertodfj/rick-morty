using System.ComponentModel.DataAnnotations;

namespace RickYMorty.dto
{
    public class GetEpisode
    {
        [Required]
        [Range(1, 51, ErrorMessage = "Id must be between 1 and 51.")]
        public int Id { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "OwnerID must be a positive integer.")]
        public int OwnerID { get; set; }  // Se agregara automaticamente en el backend

        public GetEpisode(int id, int ownerID)
        {
            Id = id;
            OwnerID = ownerID;
        }
    }
}