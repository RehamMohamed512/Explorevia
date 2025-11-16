using System.ComponentModel.DataAnnotations;

namespace Explorevia.DTOs
{
    public class RegisterDTO
    {
        [Required,MaxLength(30)]
        public string Name { get; set; }

        [EmailAddress,Required]
        public string Email { get; set; }

        [Required, MinLength(10)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Passwords do not match."),Required]
        public string ConfirmPassword { get; set; }
    }
}
