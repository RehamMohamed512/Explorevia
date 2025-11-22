using Explorevia.ViewModels;
using Explorevia.IRepository;
using Explorevia.Models;
using Explorevia.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;



namespace Explorevia.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AppDbContext _context;

        public AuthRepository(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<ApplicationUser> signInManager,
            AppDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _context = context;
        }

        // ============================
        // Register User
        // ============================
        public async Task<bool> RegisterUser(RegisterViewModel rdto)
        {
            // Check duplicate email
            var existingUser = await _userManager.FindByEmailAsync(rdto.Email);
            if (existingUser != null)
                return false;

            var newUser = new ApplicationUser
            {
                UserName = rdto.Name,
                Email = rdto.Email  
            };

            // Create user
            var result = await _userManager.CreateAsync(newUser, rdto.Password);
            if (!result.Succeeded)
                return false;

            // Create role if first time
            if (!await _roleManager.RoleExistsAsync("User"))
                await _roleManager.CreateAsync(new IdentityRole("User"));

            // Assign role to user
            await _userManager.AddToRoleAsync(newUser, "User");

            return true;
        }

        // ============================
        // Register Hotel
        // ============================
        public async Task<bool> RegisterHotelOwner(HotelOwnerRegisterViewModel hotel)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // Check duplicate email
                    var existingHotelOwner = await _userManager.FindByEmailAsync(hotel.Email);
                    if (existingHotelOwner != null)
                        return false;
                    var newHotel = new Hotel
                    {
                        Name = hotel.HotelName,
                        Location = hotel.Location,
                        Description = hotel.Description,
                        Rating = hotel.Rating
                    };
                    var appUser = new ApplicationUser
                    {
                        UserName = hotel.OwnerName,
                        Email = hotel.Email,
                        PhoneNumber = hotel.PhoneNumber
                    };
                    // Create user
                    var result = await _userManager.CreateAsync(appUser, hotel.Password);
                    if (!result.Succeeded)
                        return false;

                    // Create role if first time
                    if (!await _roleManager.RoleExistsAsync("HotelOwner"))
                        await _roleManager.CreateAsync(new IdentityRole("HotelOwner"));

                    // Assign role to user
                    await _userManager.AddToRoleAsync(appUser, "HotelOwner");
                    // Link hotel owner to user
                    newHotel.OwnerId = appUser.Id;

                    // Add hotel owner to database
                    await _context.Hotels.AddAsync(newHotel);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return true;
                }
                catch  {
                    await transaction.RollbackAsync();
                    return false;
                }
            }
        }

        // ============================
        // Login
        // ============================
        public async Task<string> Login(LoginViewModel ldto)
        {
            
            var user = await _userManager.FindByEmailAsync(ldto.Email);

            if (user == null)
                return "Envalid Email or Password";

            // Check password
            bool isPasswordCorrect = await _userManager.CheckPasswordAsync(user, ldto.Password);
            if (!isPasswordCorrect)
                return "Envalid Email or Password";

            // Sign in user
            await _signInManager.SignInAsync(user, ldto.RememberMe);

            if(await _userManager.IsInRoleAsync(user, "HotelOwner"))
            {
                return "HotelOwner";
            }
            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                return "Admin";
            }
            if (await _userManager.IsInRoleAsync(user, "User"))
            {
                return "User";
            }
           return "Envalid Email or Password";

        }

       
        public async Task<bool> LogoutAsync()
        {
            await _signInManager.SignOutAsync();
            return true;
        }
    }
}
