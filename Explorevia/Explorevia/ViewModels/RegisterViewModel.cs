using System.ComponentModel.DataAnnotations;

namespace Explorevia.ViewModels
{
    public class RegisterViewModel
    {
        [Required,MaxLength(30)]
        public string Name { get; set; }

        [EmailAddress,Required]
        public string Email { get; set; }

        [Required, MinLength(10),DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Passwords do not match."),Required, 
            DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
