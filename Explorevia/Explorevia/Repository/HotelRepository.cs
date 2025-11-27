using Explorevia.IRepository;
using Explorevia.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Explorevia.Repository
{
    public class HotelRepository : IHotelRepository
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

        

        public void getHotelDetails(Hotel hotel)
        {
            throw new NotImplementedException();
        }


        public async Task<List<Hotel>> FilterAsync(string location, int minPrice, int maxPrice, double minRating)
        {


            var query = _context.Hotels
     .Include(h => h.HotelImages)
     .Include(h => h.Reviews)
     .AsQueryable(); // for better performance "Linq method build dynamic database queries 
                     //don't execute query until i finish don't load all hotels in memory and then filter but load only filtered


            // Location filter
            if (location != "All")
                query = query.Where(h => h.City == location);

            // Price filter (only apply if not default)
            if (minPrice > 0 || maxPrice < 10000)
            {
                query = query.Where(h =>
                    h.PricePerNight >= minPrice &&
                    h.PricePerNight <= maxPrice
                );
            }

            // Rating filter (only apply if user selected one)
            if (minRating > 0)
            {
                query = query.Where(h => h.Rating >= minRating);
            }

            return await query.ToListAsync();
        }

    }
}
