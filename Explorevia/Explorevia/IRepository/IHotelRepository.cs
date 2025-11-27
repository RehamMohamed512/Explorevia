using Explorevia.Models;

namespace Explorevia.IRepository
{
    public interface IHotelRepository
    {
        Task<List<Hotel>> GetAll();
        Hotel GetById(int id);
        //void AddHotel(Hotel hotel);
        Task<List<Hotel>> FilterAsync(string location, int minPrice, int maxPrice, double minRating);
        void UpdateHotel(Hotel hotel);
        void DeleteHotel(int id);
        void AddHotelImage(HotelImage image);
        void getHotelDetails(Hotel hotel);
    }
}
