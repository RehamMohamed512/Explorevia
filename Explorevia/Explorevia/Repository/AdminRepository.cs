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

        public async Task<bool> ApproveRequest(int requestId,HotelOwnerRegisterViewModel hotel)
        {
            var req = await _context.HotelRegistrationRequests.FindAsync(requestId);
            if (req == null)
                return false;
            if (req.Status != "Pending")
                return false;
            using (var transaction = await _context.Database.BeginTransactionAsync())

                try
                {
                    var existingHotelOwner = await _userManager.FindByEmailAsync(hotel.Email);
                    if (existingHotelOwner != null)
                        return false;
                    // Create user
                    var appUser = new ApplicationUser
                    {
                        UserName = hotel.OwnerName,
                        Email = hotel.Email,
                        PhoneNumber = hotel.PhoneNumber
                    };
                    var result = await _userManager.CreateAsync(appUser, hotel.Password);
                    if (!result.Succeeded)
                        return false;

                    // Create role if first time
                    if (!await _roleManager.RoleExistsAsync("HotelOwner"))
                        await _roleManager.CreateAsync(new IdentityRole("HotelOwner"));

                    // Assign role to user
                    await _userManager.AddToRoleAsync(appUser, "HotelOwner");


                    var newHotel = new Hotel
                    {
                        Name = hotel.HotelName,
                        Description = hotel.Description,
                        Rating = hotel.Rating,
                        Address = hotel.Address,
                        City = hotel.City,
                        Country = hotel.Country

                    };

                    // Link hotel owner to user
                    newHotel.OwnerId = appUser.Id;

                    // Add hotel owner to database
                    await _context.Hotels.AddAsync(newHotel);
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

        
    }
}
