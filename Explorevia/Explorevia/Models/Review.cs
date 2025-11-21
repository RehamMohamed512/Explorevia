using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Explorevia.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }

        [Range(1, 5),Required]
        public int Rating { get; set; }

        [MaxLength(500)]
        public string Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public bool IsApproved { get; set; } = false;

        [ForeignKey("User")]
        public string UserId { get; set; }
<<<<<<< HEAD
        public ApplicationUser? User { get; set; }
=======
        public ApplicationUser User { get; set; }
>>>>>>> 5dcde25b1f1c760085716d479c40839990988c32

        [ForeignKey("Hotel")]
        public int HotelId { get; set; }
        public Hotel? Hotel { get; set; }
    }
}
