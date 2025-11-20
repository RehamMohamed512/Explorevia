using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using System.ComponentModel.DataAnnotations;

namespace Explorevia.Models
{
    public class ApplicationUser : IdentityUser
    {
<<<<<<< HEAD
        // public int Id { get; set; }
=======
        public int Id { get; set; }
>>>>>>> 371f56d96280209b8db5c5c7f6bac9aa137b8cfb
        /*  [Key]


          [Required, MaxLength(100)]
          public string Name { get; set; }

          [Required, MaxLength(150),EmailAddress]
          public string Email { get; set; }

          [Required]
          public string PasswordHash { get; set; } */

        [Required]
        public string Role { get; set; } = "User"; // User or Admin or HotelOwner

<<<<<<< HEAD

=======
>>>>>>> 371f56d96280209b8db5c5c7f6bac9aa137b8cfb
        public virtual ICollection<Booking>? Bookings { get; set; }
        public virtual ICollection<Review>? Reviews { get; set; }

    }
}
