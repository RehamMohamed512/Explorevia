using Explorevia.IRepository;
using Explorevia.Models;

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

        public async Task<bool> Requests()
        {
           var req = _context.HotelRegistrationRequests.Where(r => r.Status == "Pending").ToList();
            if (req != null && req.Count > 0)
            {
                return true;
            }
            return false;
        } 
        public async Task<bool> GetDetails(int Id)
        {
            var req = _context.HotelRegistrationRequests.FindAsync(Id);
            if (req != null)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> ApproveRequest(int requestId)
        {
           var req = _context.HotelRegistrationRequests.FindAsync(requestId);
            if (req == null)
                return false;
            req.Result.Status = "Approved";
            await _context.SaveChangesAsync();
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
