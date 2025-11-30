using System.ComponentModel.DataAnnotations;

namespace Explorevia.ViewModels
{
    public class HotelOwnerRegisterViewModel
    {
        [Required(ErrorMessage = "*"), Display(Name = "Hotel Name")]
        public string HotelName { get; set; }
        //-----------------------------------------------------
        [Required(ErrorMessage = "*")]
        public string OwnerName { get; set; }
        
     
        //-----------------------------------------------------
        [Required(ErrorMessage = "*"), EmailAddress]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.(com)$",
        ErrorMessage = "Please enter a valid email ending with .com")]
        public string Email { get; set; }

        //-----------------------------------------------------
        [DataType(DataType.Password), Required]
        public string Password { get; set; }
        //-----------------------------------------------------


        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        //-----------------------------------------------------
        //[Required(ErrorMessage = "*")]
        //public string Description { get; set; }
        //-----------------------------------------------------
        [Required(ErrorMessage = "*")]
        public string City { get; set; }
        //-----------------------------------------------------

        [Required(ErrorMessage = "*")]
        public string Country { get; set; }

        [Required]
        public string Address { get; set; }


        //-----------------------------------------------------
        [Required]
        public IFormFile HotelLicense { get; set; }
        [Required]
        public IFormFile OwnerIdCard { get; set; }
        public List<IFormFile>? Images { get; set; }
    }
}
