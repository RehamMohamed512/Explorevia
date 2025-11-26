using Explorevia.Models;
using Microsoft.EntityFrameworkCore;

namespace Explorevia.Repository
{
    public class HotelRepository
    {
        private readonly AppDbContext _context;
        public HotelRepository(AppDbContext context) => _context = context;

        public async Task<List<Hotel>> GetAll()
        {
            return await _context.Hotels
                     .Include(h => h.HotelImages)
                     .Include(h => h.Reviews)
                     .ToListAsync();
        }

        public Hotel GetById(int id) => _context.Hotels.Include(h => h.HotelImages).Include(h => h.Reviews).FirstOrDefault(h => h.Id == id);



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
