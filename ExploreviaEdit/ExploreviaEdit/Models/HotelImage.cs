using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExploreviaEdit.Models
{
    public class HotelImage
    {

        public int Id { get; set; }
        [Required]
        public string ImageUrl { get; set; }

        [ForeignKey("Hotel")]
        public int HotelId { get; set; }
        public virtual Hotel? Hotel { get; set; }
    }
}
