using System.ComponentModel.DataAnnotations;

namespace RickYMorty.dto.auth
{
    public class RegisterDTO
    {
        [EmailAddress]
        [Required]
        public required string Email { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3)]
        public required string Username { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 10)]
        public required string Password { get; set; }
        
        [Required]
        [StringLength(100, MinimumLength = 10)]
        public required string ConfirmPassword { get; set; }
    }
}