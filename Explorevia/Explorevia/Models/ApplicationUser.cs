using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using System.ComponentModel.DataAnnotations;

namespace Explorevia.Models
{
    public class ApplicationUser : IdentityUser
    {
        //public int Id { get; set; }
        /*  [Key]


          [Required, MaxLength(100)]
          public string Name { get; set; }

          [Required, MaxLength(150),EmailAddress]
          public string Email { get; set; }

          [Required]
          public string PasswordHash { get; set; } */

        [Required]
        public string Role { get; set; } = "User"; // User or Admin or HotelOwner

        public virtual ICollection<Booking>? Bookings { get; set; }
        public virtual ICollection<Review>? Reviews { get; set; }

    }
}
