using System.ComponentModel.DataAnnotations;

namespace RickYMorty.dto
{
    public class PutItemForSaleDTO
    {
        [Required]
        [Range(1, 826, ErrorMessage = "ItemId must be greater than 0.")]
        public int ItemId { get; set; }

        [Required]
        [Range(1.0, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public double Price { get; set; }

        public PutItemForSaleDTO(int itemId, double price)
        {
            ItemId = itemId;
            Price = price;
        }
    }
}