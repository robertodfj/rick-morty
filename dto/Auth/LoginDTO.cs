using System.ComponentModel.DataAnnotations;

namespace RickYMorty.dto.auth
{
    public class LoginDTO
    {
        [EmailAddress]
        public string? Email { get; set; }

        [StringLength(20, MinimumLength = 3)]
        public string? Username { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 10)]
        public required string Password { get; set; }

        // Se añade el posible null para poder inciair sesión con email o username
        
    }
}