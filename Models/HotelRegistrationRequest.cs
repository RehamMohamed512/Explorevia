using System.ComponentModel.DataAnnotations;

namespace Explorevia.Models
{
    public class HotelRegistrationRequest
    {
        public int Id { get; set; }
        [Required,MaxLength(30)]
        public string HotelName { get; set; }
        [Required]
        public string OwnerName { get; set; }
        [Required,EmailAddress]
        public string Email { get; set; }

        [Required,DataType(DataType.Password)]
        public string Password { get; set; }
        //[Required]
        //public string Description { get; set; }
        [Required,Range(1,7)]
        public double Rating { get; set; }
        
        [Required]
        public string Address { get; set; }

        [Required,MaxLength(20)]
        public string City { get; set; }

        [Required,MaxLength(20)]
        public string Country { get; set; }
        [Required,DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
        [Required]
        public string HotelLicensePath { get; set; }
        [Required]
        public string OwnerIdCardPath { get; set; }

        public string ImagesJson { get; set; }

        public string Status { get; set; } = "Pending"; //approved, rejected, pending

        public DateTime CreatedAt { get; set; } = DateTime.Now;

    }
}
