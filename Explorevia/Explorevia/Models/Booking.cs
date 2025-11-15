using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Explorevia.Models
{
    public class Booking
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required,Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }

        public string Status { get; set; } = "Pending"; // Pending, Confirmed, Cancelled

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; } 

        [ForeignKey("Room")]
        public int RoomId { get; set; }
        public Room Room { get; set; } 

        [ForeignKey("Hotel")]
        public int HotelId { get; set; }
        public Hotel Hotel { get; set; } 
    }
}
