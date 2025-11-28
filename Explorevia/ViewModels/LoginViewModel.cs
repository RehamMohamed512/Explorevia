using System.ComponentModel.DataAnnotations;

namespace Explorevia.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email format")]
        public string Email { get; set; }
        [DataType(DataType.Password),Required(ErrorMessage = "Password is required")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d).{8,}$",
            ErrorMessage = "Password must be at least 8 characters, include 1 uppercase letter and 1 number")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }


    }
}
