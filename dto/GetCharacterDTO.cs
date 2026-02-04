using System.ComponentModel.DataAnnotations;

namespace RickYMorty.dto
{
    public class GetCharacterDTO
    {
        [Required]
        [Range(1, 826, ErrorMessage = "Id must be between 1 and 826.")] // Maximo de personajes en la API de Rick and Morty
        public int Id { get; set; }
        public int OwnerID { get; set; } // Se agregara automaticamente en el backend

        public GetCharacterDTO(int id, int ownerID)
        {
            Id = id;
            OwnerID = ownerID;
        }
    }
}