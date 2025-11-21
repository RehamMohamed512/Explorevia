using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Explorevia.Models
{
    public class ApplicationUser : IdentityUser
    {
        // public int Id { get; set; }

        [Required]
        public string Role { get; set; } = "User"; // User or Admin or HotelOwner

        public virtual ICollection<Booking>? Bookings { get; set; }
        public virtual ICollection<Review>? Reviews { get; set; }
    }
}
