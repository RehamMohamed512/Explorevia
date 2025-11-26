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
        private readonly IWebHostEnvironment _environment;
        public AuthRepository(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<ApplicationUser> signInManager,
            AppDbContext context,
            IWebHostEnvironment environment)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _context = context;
            _environment = environment;
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
        public async Task<bool> SendHotelRegisterRequest(HotelOwnerRegisterViewModel hotel)
        {
            
            if (hotel == null) return false;
            if (hotel.HotelLicense == null || hotel.OwnerIdCard == null) return false;

            var licenseName = Guid.NewGuid().ToString() + Path.GetExtension(hotel.HotelLicense.FileName);
            var idCardName = Guid.NewGuid().ToString() + Path.GetExtension(hotel.OwnerIdCard.FileName);
            var imagesNames = new List<string>();

            if (hotel.Images != null && hotel.Images.Count > 0)
            {
                foreach (var image in hotel.Images)
                {
                    if (image == null) continue;
                    imagesNames.Add(Guid.NewGuid().ToString() + Path.GetExtension(image.FileName));
                }
            }


            var savePath = Path.Combine(_environment.WebRootPath ?? "wwwroot", "HotelRequests");
            if (!Directory.Exists(savePath))
                Directory.CreateDirectory(savePath);

            var savedFilePaths = new List<string>(); 

            try
            {

                var licensePath = Path.Combine(savePath, licenseName);
                using (var stream = new FileStream(licensePath, FileMode.Create))
                {
                    await hotel.HotelLicense.CopyToAsync(stream);
                }
                savedFilePaths.Add(licensePath);

                var idCardPath = Path.Combine(savePath, idCardName);
                using (var stream = new FileStream(idCardPath, FileMode.Create))
                {
                    await hotel.OwnerIdCard.CopyToAsync(stream);
                }
                savedFilePaths.Add(idCardPath);

                if (hotel.Images != null && hotel.Images.Count > 0)
                {
                    for (int i = 0; i < hotel.Images.Count; i++)
                    {
                        var imagePath = Path.Combine(savePath, imagesNames[i]);
                        using (var stream = new FileStream(imagePath, FileMode.Create))
                        {
                            await hotel.Images[i].CopyToAsync(stream);
                        }
                        savedFilePaths.Add(imagePath);
                    }
                }

                var newRequest = new HotelRegistrationRequest
                {
                    OwnerName = hotel.OwnerName,
                    HotelName = hotel.HotelName,
                    Email = hotel.Email,
                    Password = hotel.Password,
                    Phone = hotel.PhoneNumber,
                    Description = hotel.Description,
                    City = hotel.City,
                    Country = hotel.Country,
                    Address = hotel.Address,
                    Rating = hotel.Rating,
                    HotelLicensePath = Path.Combine("HotelRequests", licenseName),    // تخزين المسار النسبي
                    OwnerIdCardPath = Path.Combine("HotelRequests", idCardName),
                    ImagesJson = System.Text.Json.JsonSerializer.Serialize(imagesNames.Select(n => Path.Combine("HotelRequests", n))),
                    Status = "Pending",
                    CreatedAt = DateTime.Now
                };

                await _context.HotelRegistrationRequests.AddAsync(newRequest);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                foreach (var p in savedFilePaths)
                {
                    try
                    {
                        if (File.Exists(p)) File.Delete(p);
                    }
                    catch(Exception) { }
                }

                return false;
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
            if (await _userManager.IsInRoleAsync(user,"User"))
            {
                return "User";
            }
           return "Envalid Email or Password";

        }

        // ============================
        // Logout
        // ============================
        public async Task<bool> LogoutAsync()
        {
            await _signInManager.SignOutAsync();
            return true;
        }
    }
}
