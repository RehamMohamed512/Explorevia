using Explorevia.IRepository;
using Explorevia.Models;
using Explorevia.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Explorevia.Repository
{
    public class AdminRepository : IAdminRepository
    {
        //DI
        private readonly AppDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private UserManager<ApplicationUser> _userManager;
        IAuthRepository authRepository;
        public AdminRepository(AppDbContext context, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, IAuthRepository authRepository)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
            this.authRepository = authRepository;
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
        public async Task<List<Hotel>> GetAllHotels()
        {
            var hotels = await _context.Hotels.ToListAsync();
            if(hotels != null)
            {
                return hotels;
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
            var req = await _context.HotelRegistrationRequests.FindAsync(requestId);
            if (req == null || req.Status != "Pending")
                return false;

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {

                var appUser = new ApplicationUser
                {
                    UserName = req.OwnerName,
                    Email = req.Email,
                    PhoneNumber = req.Phone
                };

                var createUser = await _userManager.CreateAsync(appUser, req.Password);
                if (!createUser.Succeeded)
                    return false;


                if (!await _roleManager.RoleExistsAsync("HotelOwner"))
                    await _roleManager.CreateAsync(new IdentityRole("HotelOwner"));


                await _userManager.AddToRoleAsync(appUser, "HotelOwner");


                var hotel = new Hotel
                {
                    Name = req.HotelName,
                    Description = req.Description,
                    Rating = req.Rating,
                    Address = req.Address,
                    City = req.City,
                    Country = req.Country,
                    OwnerId = appUser.Id,
                };

                await _context.Hotels.AddAsync(hotel);
                req.Status = "Approved";

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }


        public async Task<bool> RejectRequest(int requestId)
        {
            var req =await _context.HotelRegistrationRequests.FindAsync(requestId);
            if (req == null)
                return false;
            if (req.Status == "Approved")
                return false;
            req.Status = "Rejected";
            await _context.SaveChangesAsync();

            return true;

        }

        public async Task<bool> DeleteHotel(int id)
        {
            var hotel = await _context.Hotels.FindAsync(id);
            if (hotel == null)
                return false;
            var owner = await _userManager.FindByIdAsync(hotel.OwnerId);
            if (owner != null)
            {
                var result = await _userManager.DeleteAsync(owner);
                if (!result.Succeeded)
                    return false;
            }
            _context.Hotels.Remove(hotel);
            await _context.SaveChangesAsync();
            return true;
        }


    }
}
