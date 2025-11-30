using Explorevia.Models;

namespace Explorevia.IRepository
{
    public interface IHotelRepository
    {
        Task<List<Hotel>> GetAll();
        Hotel GetById(int id);
        
        Task<List<Hotel>> FilterAsync(string location, int minPrice, int maxPrice, double minRating);
        Task<Hotel> UpdateAsync(Hotel hotel);
        Task AddImagesAsync(List<HotelImage> images);
    }
}
