using ExploreviaEdit.IRepository;
using ExploreviaEdit.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExploreviaEdit.Repository
{
    public class AdminRepository : IAdminRepository
    {
        private readonly AppDbContext _context;
        public AdminRepository(AppDbContext context) => _context = context;

        // Pending Requests
       
        public async Task<List<HotelRegistrationRequest>> GetPendingRequests()
        {
            return await _context.HotelRegistrationRequests
                                 .Where(r => r.Status == "Pending")
                                 .ToListAsync();
        }

        public async Task<bool> ApproveRequest(int requestId)
        {
            var req = await _context.HotelRegistrationRequests.FindAsync(requestId);
            if (req == null) return false;

            req.Status = "Approved";

            // Create hotel automatically
            var hotel = new Hotel
            {
                Name = req.HotelName,
                Location = req.Address,
                Description = "Newly approved hotel",
                OwnerId = "", // Assign the owner's user id if available
                PricePerNight = 0,
                Rating = 0
            };

            _context.Hotels.Add(hotel);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RejectRequest(int requestId)
        {
            var req = await _context.HotelRegistrationRequests.FindAsync(requestId);
            if (req == null) return false;

            req.Status = "Rejected";
            await _context.SaveChangesAsync();
            return true;
        }

        
        // Hotels CRUD
      
        public async Task<List<Hotel>> GetHotels() => await _context.Hotels.ToListAsync();

        public async Task<Hotel> GetHotelById(int id) => await _context.Hotels.FindAsync(id);

        public async Task<bool> AddHotel(Hotel hotel)
        {
            _context.Hotels.Add(hotel);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateHotel(Hotel hotel)
        {
            var existing = await _context.Hotels.FindAsync(hotel.Id);
            if (existing == null) return false;

            existing.Name = hotel.Name;
            existing.Location = hotel.Location;
            existing.Description = hotel.Description;
            existing.PricePerNight = hotel.PricePerNight;
            existing.Rating = hotel.Rating;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteHotel(int id)
        {
            var hotel = await _context.Hotels.FindAsync(id);
            if (hotel == null) return false;

            _context.Hotels.Remove(hotel);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
