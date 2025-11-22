using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ExploreviaEdit.Models
{
    public class ApplicationUser: IdentityUser
    {
        [Required]
        public string Role { get; set; } = "User"; // User or Admin or HotelOwner

        public virtual ICollection<Booking>? Bookings { get; set; }
        public virtual ICollection<Review>? Reviews { get; set; }
    }
}
