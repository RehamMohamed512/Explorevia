using System.ComponentModel.DataAnnotations;

namespace Explorevia.Models
{
    public class Hotel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(150)]
        public string Name { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]

        public int Rating { get; set; }
        [Required]
        public decimal PricePerNight { get; set; }

        public virtual ICollection<BookingRequests> BookingRequests { get; set; }
    }
}
