using System.ComponentModel.DataAnnotations;

namespace Explorevia.DTOs
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "*")]
        public string Email { get; set; }
        [DataType(DataType.Password),Required]
        public string Password { get; set; }

        public bool RememberMe { get; set; }

    }
}
