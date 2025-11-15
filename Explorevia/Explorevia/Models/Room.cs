using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Explorevia.Models
{
    public class Room
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string RoomType { get; set; }

        [Required,Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        public bool IsAvailable { get; set; } = true;

        [ForeignKey("Hotel")]
        public int HotelId { get; set; }
        public Hotel Hotel { get; set; }

        public ICollection<Booking> Bookings { get; set; }
    }
}
