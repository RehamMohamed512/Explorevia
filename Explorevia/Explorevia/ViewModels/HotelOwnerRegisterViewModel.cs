using System.ComponentModel.DataAnnotations;

namespace Explorevia.ViewModels
{
    public class HotelOwnerRegisterViewModel
    {
        [Required(ErrorMessage = "*")]
        public string OwnerName { get; set; }
        //-----------------------------------------------------

        [Required(ErrorMessage = "*"), Display(Name = "Hotel Name")]
        public string HotelName { get; set; }
        //-----------------------------------------------------
        [Required(ErrorMessage = "*"), EmailAddress]
        public string Email { get; set; }

        //-----------------------------------------------------
        [DataType(DataType.Password), Required]
        public string Password { get; set; }
        //-----------------------------------------------------

        [DataType(DataType.Password),Compare("Password"),Required]
        public string ConfirmPassword { get; set;}
        //-----------------------------------------------------

        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        //-----------------------------------------------------
        [Required(ErrorMessage = "*")]
        public string Description { get; set; }
        //-----------------------------------------------------
        [Required(ErrorMessage = "*")]
        public string Location { get; set; }
        //-----------------------------------------------------
        [Required(ErrorMessage = "*"),Range(1,7)]
        public double Rating { get; set; }
    }
}
