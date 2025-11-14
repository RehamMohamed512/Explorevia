using Microsoft.AspNetCore.Mvc.ViewEngines;
using System.ComponentModel.DataAnnotations;

namespace Explorevia.Models
{
    public class Hotel
    {
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Name { get; set; }

        [Required, MaxLength(200)]
        public string Location { get; set; }

        public string Description { get; set; }

        [Range(0, 5)]
        public double Rating { get; set; }

        public decimal BasePrice { get; set; }

        public ICollection<Room> Rooms { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<HotelImage> HotelImages { get; set; }
    }
}
