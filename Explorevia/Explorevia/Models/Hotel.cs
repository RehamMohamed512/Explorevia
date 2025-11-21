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

        [Range(0, 7)]
        public double Rating { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal PricePerNight { get; set; }

        [ForeignKey("User")]
        public string OwnerId { get; set; }
        public virtual ApplicationUser? Owner { get; set; }
<<<<<<< HEAD
=======

>>>>>>> 5dcde25b1f1c760085716d479c40839990988c32
        public virtual ICollection<Room>? Rooms { get; set; }
        public virtual ICollection<Booking>? Bookings { get; set; }
        public virtual ICollection<Review>? Reviews { get; set; }
        public virtual ICollection<HotelImage>? HotelImages { get; set; } 
    }
}
