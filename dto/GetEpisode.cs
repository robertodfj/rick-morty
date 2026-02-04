using System.ComponentModel.DataAnnotations;

namespace RickYMorty.dto
{
    public class GetEpisode
    {
        [Required]
        [Range(1, 51, ErrorMessage = "Id must be between 1 and 51.")] // Maximo de episodios en la API de Rick and Morty
        public int Id { get; set; }
        public int OwnerID { get; set; } // Se agregara automaticamente en el backend

        public GetEpisode(int id, int ownerID)
        {
            Id = id;
            OwnerID = ownerID;
        }
    }
}