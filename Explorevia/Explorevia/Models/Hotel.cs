using Microsoft.AspNetCore.Mvc.ViewEngines;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Explorevia.Models
{
    public class Hotel
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Name { get; set; }

        [Required, MaxLength(200)]
        public string Location { get; set; }

        [Required]
        public string Description { get; set; }

        [Range(0, 5)]
        public double Rating { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal PricePerNight { get; set; }

        [ForeignKey("User")]
        public int OwnerId { get; set; }
        public User Owner { get; set; }

        public ICollection<Room> Rooms { get; set; }
        public ICollection<Booking> Bookings { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<HotelImage> HotelImages { get; set; } 
    }
}
