using Explorevia.Models;

namespace Explorevia.IRepository
{
    public interface IHotelRepository
    {
        Task<List<Hotel>> GetAll();
        Hotel GetById(int id);
<<<<<<< HEAD
        void AddHotel(Hotel hotel);

        Task<List<Hotel>> FilterAsync(string location, int minPrice, int maxPrice, double minRating);

=======
>>>>>>> 380c7a80300a8409f8dd8b5b23a9342774243374
        void UpdateHotel(Hotel hotel);
        void DeleteHotel(int id);
        void AddHotelImage(HotelImage image);
        void getHotelDetails(Hotel hotel);
    }
}
