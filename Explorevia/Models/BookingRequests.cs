using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Explorevia.Models
{
    public class BookingRequests
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        public virtual IdentityUser User { get; set; }
        [Required]
        [ForeignKey("Hotel")]
        public int HotelId { get; set; }
        public virtual Hotel Hotel { get; set; }

        public DateTime CheckInDate { get; set; } = DateTime.Now;
        public DateTime CheckOutDate { get; set; }

        public int NumberOfGuests { get; set; }
        public string Status { get; set; } = "Pending";// e.g., Pending, Confirmed, Cancelled

       

    }
}
