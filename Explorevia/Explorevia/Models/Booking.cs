using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Explorevia.Models
{
    public class Booking
    {
        public int Id { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public decimal TotalPrice { get; set; }

        [Required]
        public string Status { get; set; } = "Pending"; // Pending, Confirmed, Cancelled

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }

        [ForeignKey("Room")]
        public int RoomId { get; set; }
        public Room Room { get; set; }

        public Payment Payment { get; set; }

        public string BookingCode { get; set; } = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
    }
}
