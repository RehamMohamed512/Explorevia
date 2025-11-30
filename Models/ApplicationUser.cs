using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Explorevia.Models
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<Booking>? Bookings { get; set; }
        public virtual ICollection<Review>? Reviews { get; set; }
    }
}
