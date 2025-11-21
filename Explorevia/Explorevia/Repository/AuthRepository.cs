using Explorevia.DTOs;
using Explorevia.IRepository;
using Explorevia.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;



namespace Explorevia.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthRepository(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
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
                UserName = rdto.Email,
                Email = rdto.Email,
                Role = "User"
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
        // Login
        // ============================
        public async Task<bool> Login(LoginViewModel ldto)
        {
            var user = await _userManager.FindByEmailAsync(ldto.Email);

            if (user == null)
                return false;

            // Check password
            bool isPasswordCorrect = await _userManager.CheckPasswordAsync(user, ldto.Password);
            if (!isPasswordCorrect)
                return false;

            // Sign in user
            await _signInManager.SignInAsync(user, ldto.RememberMe);
            return true;
        }

       
        public async Task<bool> LogoutAsync()
        {
            await _signInManager.SignOutAsync();
            return true;
        }
    }
}
