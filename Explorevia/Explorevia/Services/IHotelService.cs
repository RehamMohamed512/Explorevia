using Explorevia.Models;
using Microsoft.EntityFrameworkCore;


namespace ExploreVia.Services
{
    public interface IHotelService
    {
        IEnumerable<Hotel> GetAll();
        Hotel GetById(int id);
        void AddHotel(Hotel hotel);
        void UpdateHotel(Hotel hotel);
        void DeleteHotel(int id);
        void AddHotelImage(HotelImage image);
    }

    public class HotelService : IHotelService
    {
        private readonly AppDbContext _context;
        public HotelService(AppDbContext context) => _context = context;

        public IEnumerable<Hotel> GetAll() => _context.Hotels.Include(h => h.HotelImages).Include(h => h.Reviews).ToList();

        public Hotel GetById(int id) => _context.Hotels.Include(h => h.HotelImages).Include(h => h.Reviews).FirstOrDefault(h => h.Id == id);

        public void AddHotel(Hotel hotel)
        {
            _context.Hotels.Add(hotel);
            _context.SaveChanges();
        }

        public void UpdateHotel(Hotel hotel)
        {
            _context.Hotels.Update(hotel);
            _context.SaveChanges();
        }

        public void DeleteHotel(int id)
        {
            var hotel = _context.Hotels.Find(id);
            if (hotel != null) { _context.Hotels.Remove(hotel); _context.SaveChanges(); }
        }

        public void AddHotelImage(HotelImage image)
        {
            _context.HotelImages.Add(image);
            _context.SaveChanges();
        }
    }
}

