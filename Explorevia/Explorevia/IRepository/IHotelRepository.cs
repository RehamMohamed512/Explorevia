using Explorevia.Models;

namespace Explorevia.IRepository
{
    public interface IHotelRepository
    {
        IEnumerable<Hotel> GetAll();
        Hotel GetById(int id);
        void AddHotel(Hotel hotel);
        void UpdateHotel(Hotel hotel);
        void DeleteHotel(int id);
        void AddHotelImage(HotelImage image);
    }
}
