using Explorevia.Models;
using System.ComponentModel.DataAnnotations;

namespace Explorevia.ViewModels
{
    public class HotelVieModel
    {

        public int? HotelId { get; set; }

        public string HotelName { get; set; }
        public decimal Price { get; set; }
        public string Rating { get; set; }
        public string Description { get; set; }

        // update images
        public List<IFormFile>? NewImages { get; set; }

        // existing images 
        public List<HotelImage>? ExistingImages { get; set; }

    }
}
