using System.ComponentModel.DataAnnotations;

namespace RickYMorty.dto
{
    public class GetCharacterDTO
    {
        [Required]
        [Range(1, 826, ErrorMessage = "Id must be between 1 and 826.")] 
        public int Id { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "OwnerID must be a positive integer.")] 
        public int OwnerID { get; set; } // Se agregara automaticamente en el backend

        public GetCharacterDTO(int id, int ownerID)
        {
            Id = id;
            OwnerID = ownerID;
        }
    }
}