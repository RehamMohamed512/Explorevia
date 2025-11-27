using Explorevia.IRepository;
using Explorevia.Models;
using Microsoft.EntityFrameworkCore;

namespace Explorevia.Repository
{
    public class AdminRepository : IAdminRepository
    {
        //DI
        private readonly AppDbContext _context;
        public AdminRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<HotelRegistrationRequest>> Requests()
        {
           var req = await _context.HotelRegistrationRequests.Where(r => r.Status == "Pending").ToListAsync();
            if(req != null)
            {
                return req;
            }
            return null;
        } 
        public async Task<HotelRegistrationRequest> GetDetails(int Id)
        {
            var req = await _context.HotelRegistrationRequests.FindAsync(Id);
            if(req != null)
            {
                return req;
            }
            return null;
        }

        public async Task<bool> ApproveRequest(int requestId)
        {
           var req = _context.HotelRegistrationRequests.FindAsync(requestId);
            if (req == null)
                return false;
            var hotel = new Hotel
            {
                Name = req.Result.HotelName,
                Description = req.Result.Description,
              //  City = req.Result.City,
              //  Country = req.Result.Country,
                Location = req.Result.Address,
                Rating = req.Result.Rating,
                
            };
            await _context.Hotels.AddAsync(hotel);
            await _context.SaveChangesAsync();

            var image = new List<string>();

            return true;
        }

        public async Task<bool> RejectRequest(int requestId)
        {
            var req = _context.HotelRegistrationRequests.FindAsync(requestId);
            if (req == null)
                return false;
            req.Result.Status = "Rejected";
            await _context.SaveChangesAsync();
            return true;

        }

        
    }
}
