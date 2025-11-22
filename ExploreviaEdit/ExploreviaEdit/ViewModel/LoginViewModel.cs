using System.ComponentModel.DataAnnotations;

namespace ExploreviaEdit.ViewModel
{
    public class LoginViewModel
    {
        public LoginViewModel() { }  // required

        [Required(ErrorMessage = "*")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "*")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
