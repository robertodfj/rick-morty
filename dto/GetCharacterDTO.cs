using System.ComponentModel.DataAnnotations;

namespace RickYMorty.dto
{
    public class GetCharacterDTO
    {
        [Required]
        [Range(1, 826, ErrorMessage = "Id must be between 1 and 826.")] 
        public int Id { get; set; }


        public GetCharacterDTO(int id)
        {
            Id = id;
        }
    }
}