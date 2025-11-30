using System.ComponentModel.DataAnnotations;

namespace Explorevia.ViewModels
{
    public class HotelVieModel
    {

        [Key]
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Name { get; set; }

    }
}
